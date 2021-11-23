using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Suggession.Data;
using Suggession.Models;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suggession.DTO;

namespace Suggession.Controllers
{
    public class UploadFileController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly DataContext _context;

        public UploadFileController(IWebHostEnvironment currentEnvironment, DataContext context)
        {
            _currentEnvironment = currentEnvironment;
            _context = context;
        }

        [AcceptVerbs("Post")]
        public void Save(int kpiId, DateTime uploadTime)
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var httpPostedFile = HttpContext.Request.Form.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            using (Stream fileStream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(fileStream);

                            }

                            var item = new UploadFile();
                            item.Path = $"/UploadedFiles/{httpPostedFile.FileName}";
                            item.IdealID = kpiId;
                            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
                            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                            item.UploadTime = new DateTime(year, month, 1);
                            item.CreatedTime = DateTime.MinValue;
                            _context.UploadFiles.Update(item);
                            _context.SaveChanges();
                          
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File uploaded succesfully";

                         
                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 400;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 400;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [AcceptVerbs("Post")]
        public void Remove(int kpiId, DateTime uploadTime)
        {
           
            try
            {
                var fileSave = "";
                if (HttpContext.Request.Form["cancel-uploading"]== false)
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadingFiles");
                }
                else
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                }
                var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
                var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                var ut = new DateTime(year, month, 1);

                var fileName = HttpContext.Request.Form["UploadFiles"];

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }
              

                string path = $"/UploadedFiles/{fileName.First()}";
                var item = _context.UploadFiles.FirstOrDefault(x => x.IdealID == kpiId && x.Path == path && x.UploadTime == ut);
                _context.UploadFiles.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";
               
            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 400;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Falied to remove!";
            }
        }

        [HttpGet]
        public IActionResult Download(int kpiId, DateTime uploadTime)
        {

            try
            {
                var (fileType, archiveData, archiveName) = DownloadFiles(kpiId,uploadTime);

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAttackFiles(int ideaId)
        {
            var ideadID = _context.IdeaHistories.FirstOrDefault(x => x.IdeaID == ideaId).Id;
            var data = await _context.UploadFiles.Where(x => x.IdealID == ideadID).ToListAsync();
            var files = data.Select(x=> x.Path ).ToList();
            var fileInfoList = new List<PreloadFileDto>();
                files.ForEach(file =>
                    {
                        string filePath = _currentEnvironment.WebRootPath + file;
                        if (System.IO.File.Exists(filePath))
                        {
                            var info = new FileInfo(filePath);
                            fileInfoList.Add(new PreloadFileDto
                            {
                                Name = Path.GetFileNameWithoutExtension(filePath) + info.Extension,
                                Size = info.Length,
                                Type = info.Extension

                            });
                        }
                    });
            return Ok(fileInfoList);
        }
        [HttpGet]
        public async Task<IActionResult> GetAttackFilesIdea(int ideaId)
        {
            var NullData = new List<DownloadFileDto>();
            var ideadID = _context.IdeaHistories.FirstOrDefault(x => x.IdeaID == ideaId) != null ? _context.IdeaHistories.FirstOrDefault(x => x.IdeaID == ideaId).Id : 0;
            var data = await _context.UploadFiles.Where(x => x.IdealID == ideadID).ToListAsync();
            if (data.Count == 0)
                return Ok(NullData);
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file,
                    IdeaId = ideadID

                });
            });
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttackFilesIdeaPreview(int ideaId)
        {
            var NullData = new List<DownloadFileDto>();
            var ideadID = ideaId;
            var data = await _context.UploadFiles.Where(x => x.IdealID == ideadID).ToListAsync();
            if (data.Count == 0)
                return Ok(NullData);
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file,
                    IdeaId = ideadID

                });
            });
            return Ok(list);
        }
        [HttpDelete("{ideaId}/{file}")]
        public void RemoveFileIdea(int ideaId, string file)
        {

            try
            {
                var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");

                var fileName = file;

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }


                string path = $"/UploadedFiles/{fileName}";
                var item = _context.UploadFiles.FirstOrDefault(x => x.IdealID == ideaId && x.Path == path);
                _context.UploadFiles.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";

            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 400;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Falied to remove!";
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDownloadFiles(int kpiId, DateTime uploadTime)
        {
            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = await _context.UploadFiles.Where(x => x.IdealID == kpiid && x.UploadTime == ut).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetDownloadFilesIdea(int ideaID)
        {

            var data = await _context.UploadFiles.Where(x => x.IdealID == ideaID).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file,
                    IdeaId = ideaID

                });
            });
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetDownloadFilesMeeting(int kpiId, DateTime uploadTime)
        {
            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = await _context.UploadFiles.Where(x => x.IdealID == kpiid && x.UploadTime == ut).ToListAsync();
            var files = data.Select(x => x.Path).ToList();
            var list = new List<DownloadFileDto>();
            files.ForEach(file =>
            {
                string filePath = _currentEnvironment.WebRootPath + file;
                var info = new FileInfo(filePath);
                list.Add(new DownloadFileDto
                {
                    Name = Path.GetFileName(filePath),
                    Path = file

                });
            });
            return Ok(list);
        }

        #region Download File  
        private (string fileType, byte[] archiveData, string archiveName) DownloadFiles(int kpiId , DateTime uploadTime)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = _context.UploadFiles.Where(x => x.IdealID == kpiid && x.UploadTime == ut).ToList();
            var files = data.Select(x=> x.Path ).ToList();
            
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        string filePath = _currentEnvironment.WebRootPath + file;
                        archive.CreateEntryFromFile(filePath, System.IO.Path.GetFileName(filePath));
                      
                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/octet-stream"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        #endregion
    }
}
