using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using NetUtility;
using OfficeOpenXml;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Models;
using Suggession.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Suggession.Controllers
{
    public class ToDoListController : ApiControllerBase
    {
        private readonly IToDoListService _service;
        private readonly IAccountService _serviceAccount;
        private readonly IObjectiveService _serviceObjective;
        public readonly IWebHostEnvironment _environment;
        public ToDoListController(IToDoListService service,
            IAccountService serviceAccount,
            IWebHostEnvironment environment,
            IObjectiveService serviceObjective)
        {
            _service = service;
            _serviceAccount = serviceAccount;
            _environment = environment;
            _serviceObjective = serviceObjective;
        }

        [HttpGet("{filename}")]
        public ActionResult DownloadExcel(string filename)

        {
            string Files = $"wwwroot/FileUpload/{filename}";

            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);

            System.IO.File.WriteAllBytes(Files, fileBytes);

            MemoryStream ms = new MemoryStream(fileBytes);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

        }

        [HttpPost("{filename}")]
        public ActionResult DeleteFile(string filename)

        {
            string Files = $"wwwroot/FileUpload/{filename}";
            FileInfo TheFileInfo = new FileInfo(Files);
            if (TheFileInfo.Exists)
            {
                System.IO.File.Delete(Files);
            }
            return Ok();

        }


        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile formFile)
        {
            try
            {
                IFormFile file = Request.Form.Files["formFile"];
                if (file != null)
                {
                    string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = _environment.WebRootPath + "\\ExcelUpload" + $@"\{filename}";
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        Response.Clear();
                        Response.StatusCode = 204;
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                        return Ok(new { status = false, ExistFile = true });
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "No Content";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
            }
            return Content("");
        }

        [HttpPost]
        public async Task<ActionResult<IdeaDto>> ImportSubmit([FromForm] IdeaDto entity)
        {
            if (ModelState.IsValid)
            {
                var list = new List<ToDoList>();
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

          

                var issue = Request.Form["Issue"];
                var title = Request.Form["Title"];
                var receiveId = Request.Form["ReceiveID"];
                var sendId = Request.Form["SendID"];
                var suggession = Request.Form["Suggession"];

                if (file != null)
                {
                    //kiem tra da ton tai thu muc de upload file hay chua

                    //neu chua co thi tao moi
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadedFiles\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\UploadedFiles\\");
                    }

                    //lap file duoc truyen len
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var currentFile = Request.Form.Files[i];
                        using FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadedFiles\\" + currentFile.FileName);
                        await currentFile.CopyToAsync(fileStream);
                        fileStream.Flush();
                        var data = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(data);
                    }
                    entity.SendID = sendId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.File = fileupload;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.Status = Suggession.Constants.Status.Apply;
                    entity.Isshow = true;
                }else
                {
                    entity.SendID = sendId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.File = fileupload;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.Status = Suggession.Constants.Status.Apply;
                    entity.Isshow = true;
                }
                

                var model = await _service.UploadFile(entity);
                return Ok(model);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(entity);
        }
        [HttpPost]
        public async Task<ActionResult<IdeaDto>> ImportSave([FromForm] IdeaDto entity)
        {
            if (ModelState.IsValid)
            {
                var list = new List<ToDoList>();
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

             

                var issue = Request.Form["Issue"];
                var title = Request.Form["Title"];
                var receiveId = Request.Form["ReceiveID"];
                var sendId = Request.Form["SendID"];
                var suggession = Request.Form["Suggession"];

                if (file != null)
                {
                    //kiem tra da ton tai thu muc de upload file hay chua

                    //neu chua co thi tao moi
                    if (!Directory.Exists(_environment.WebRootPath + "\\FileUpload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\FileUpload\\");
                    }

                    //lap file duoc truyen len
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var currentFile = Request.Form.Files[i];
                        using FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\FileUpload\\" + currentFile.FileName);
                        await currentFile.CopyToAsync(fileStream);
                        fileStream.Flush();
                        var data = new Files
                        {
                            Path = $"/FileUpload/{currentFile.FileName}"
                        };
                        fileupload.Add(data);
                    }

                    entity.SendID = sendId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.File = fileupload;
                    entity.Status = Suggession.Constants.Status.NA;
                    entity.Isshow = true;
                } else
                {
                    entity.SendID = sendId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.File = fileupload;
                    entity.Status = Suggession.Constants.Status.NA;
                    entity.Isshow = true;
                }
                

                var model = await _service.UploadFile(entity);
                return Ok(model);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(entity);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreByAccountId()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPIScoreByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPISelfScoreByObjectiveId(int objectiveId)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GetAllKPISelfScoreByObjectiveId(objectiveId, accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreByFunctionalLeader()
        {
            return Ok(await _service.GetAllAttitudeScoreByFunctionalLeader());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL0ByPeriod(int period)
        {

            return Ok(await _service.GetAllKPIScoreL0ByPeriod(period));
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreGHRByAccountId(int accountId, DateTime currentTime)
        {

            return Ok(await _service.GetAllKPIScoreGHRByAccountId(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL1ByAccountId(int accountId, DateTime currentTime)
        {
         
            return Ok(await _service.GetAllKPIScoreL1ByAccountId(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllKPIScoreL2ByAccountId(int accountId, DateTime currentTime)
        {

            return Ok(await _service.GetAllKPIScoreL2ByAccountId(accountId, currentTime));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreL1ByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreL1ByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreL2ByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreL2ByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttitudeScoreGFLByAccountId(int accountId)
        {

            return Ok(await _service.GetAllAttitudeScoreGFLByAccountId(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GetQuarterlySetting()
        {
            return Ok(await _service.GetQuarterlySetting());
        }
        [HttpGet]
        public async Task<ActionResult> L0( DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L0(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> L1(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L1(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> FunctionalLeader(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.FunctionalLeader(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> Updater(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.Updater(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> L2(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.L2(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> FHO()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.FHO(accountId));
        }
        [HttpGet]
        public async Task<ActionResult> GHR(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GHR(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GM(DateTime currentTime)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            return Ok(await _service.GM(accountId, currentTime));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllObjectiveByL1L2()
        {
            return Ok(await _service.GetAllObjectiveByL1L2());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveIdAsync(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllByObjectiveIdAsTreeAsync(int objectiveId)
        {
            return Ok(await _service.GetAllByObjectiveIdAsTreeAsync(objectiveId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllInCurrentQuarterByObjectiveIdAsync(int objectiveId)
        {
            return Ok(await _service.GetAllInCurrentQuarterByObjectiveIdAsync(objectiveId));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.CreatedBy = accountId;
            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] ToDoListDto model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);

            model.ModifiedBy = accountId;
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpPost]
        public async Task<ActionResult> AddRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.AddRangeAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRangeAsync([FromBody] List<ToDoListDto> model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            foreach (var item in model)
            {
                item.CreatedBy = accountId;
            }
            return StatusCodeResult(await _service.UpdateRangeAsync(model));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return StatusCodeResult(await _service.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetWithPaginationsAsync(PaginationParams paramater)
        {
            return Ok(await _service.GetWithPaginationsAsync(paramater));
        }
        [HttpPost]
        public async Task<ActionResult> Import([FromForm] IFormFile file2)
        {
            IFormFile file = Request.Form.Files["UploadedFile"];
            object uploadBy = Request.Form["UploadBy"];
            var datasList = new List<ImportExcelFHO>();
            //var datasList2 = new List<UploadDataVM2>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                int userid = uploadBy.ToInt();
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.Columns;
                    var noOfRow = workSheet.Dimension.Rows;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        datasList.Add(new ImportExcelFHO()
                        {
                            KPIObjective = workSheet.Cells[rowIterator, 1].Value.ToSafetyString(),
                            UserList = workSheet.Cells[rowIterator, 2].Value.ToSafetyString(),
                        });
                    }
                }
                var list = new List<ObjectiveDto>();
                foreach (var item in datasList)
                {
                    var accountIds = new List<int>();
                    if (item.UserList.IsNullOrEmpty() == false)
                    {
                        var accountList = item.UserList.Split(',').Select(x=>x.Trim()).ToArray();
                        foreach (var username in accountList)
                        {
                            var account = await _serviceAccount.GetByUsername(username);
                            if (account != null)
                            {
                                accountIds.Add(account.Id);
                            }
                        }
                    }
                    list.Add(new ObjectiveDto { Topic = item.KPIObjective, CreatedBy = userid, AccountIdList = accountIds });
                }
                var check = await _serviceObjective.AddRangeAsync(list);
                if (check.Success == true)
                    return Ok();
                else
                {
                    return BadRequest(check);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ExcelExport()
        {
            string filename = "FHOTemplate.xlsx";
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/excelTemplate", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
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

        [HttpPut]
        public async Task<ActionResult> Reject([FromBody] ReleaseRequestDto requestDto)
        {
            return StatusCodeResult(await _service.Reject(requestDto.Ids));
        }
        [HttpPut]
        public async Task<ActionResult> DisableReject([FromBody] ReleaseRequestDto requestDto)
        {
            return StatusCodeResult(await _service.DisableReject(requestDto.Ids));
        }
        [HttpPut]
        public async Task<ActionResult> Release([FromBody] RejectRequestDto requestDto)
        {
            return StatusCodeResult(await _service.Release(requestDto.Ids));
        }
    }
}
