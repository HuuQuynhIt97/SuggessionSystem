using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession._Services.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NetUtility;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Features;
using System;
using Suggession.Helpers.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Suggession.Controllers
{
    public class IdeaController : ApiControllerBase
    {
        private readonly IIdeaService _service;
        public readonly IWebHostEnvironment _environment;
        private readonly IHubContext<SuggessionHub> _hubContext;
        public IdeaController(
            IIdeaService service, 
            IWebHostEnvironment environment,
            IHubContext<SuggessionHub> hubContext
            )
        {
            _service = service;
            _hubContext = hubContext;
            _environment = environment;
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
                }
                else
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
        public async Task<ActionResult<IdeaDto>> ImportSubmitEdit([FromForm] IdeaDto entity)
        {
            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();
                var file = Request.Form.Files["UploadedFile"];
                var issue = Request.Form["Issue"];
                var ideadId = Request.Form["IdeaID"];
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
                    entity.Id = ideadId.ToInt();
                    entity.File = fileupload;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.Status = Suggession.Constants.Status.Apply;
                    entity.Isshow = true;
                }
                else
                {
                    entity.SendID = sendId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.File = fileupload;
                    entity.Id = ideadId.ToInt();
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.Status = Suggession.Constants.Status.Apply;
                    entity.Isshow = true;
                }


                var model = await _service.EditSubmitSuggession(entity);
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
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.File = fileupload;
                    entity.Status = Suggession.Constants.Status.NA;
                    entity.Isshow = true;
                }
                else
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

        [HttpPost]
        public async Task<ActionResult<IdeaDto>> ImportSaveEdit([FromForm] IdeaDto entity)
        {
            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];
                var issue = Request.Form["Issue"];
                var ideadId = Request.Form["IdeaID"];
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
                    entity.Id = ideadId.ToInt();
                    entity.ReceiveID = receiveId.ToInt();
                    entity.Title = title;
                    entity.Issue = issue;
                    entity.Suggession = suggession;
                    entity.CreatedBy = sendId.ToInt();
                    entity.File = fileupload;
                    entity.Status = Suggession.Constants.Status.NA;
                    entity.Isshow = true;
                }
                else
                {
                    entity.Id = ideadId.ToInt();
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


                var model = await _service.EditSuggession(entity);
                return Ok(model);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(entity);
        }

        [HttpGet]
        public async Task<ActionResult> TabProposalGetAll()
        {
            return Ok((await _service.TabProposalGetAll()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetIdeaHisById(int id)
        {
            return Ok((await _service.GetIdeaHisById(id)));
        }
        [HttpGet]
        public async Task<ActionResult> TabProcessingGetAll()
        {
            return Ok((await _service.TabProcessingGetAll()));
        }
        [HttpGet]
        public async Task<ActionResult> TabErickGetAll()
        {
            return Ok((await _service.TabErickGetAll()));
        }
        [HttpGet]
        public async Task<ActionResult> TabCloseGetAll()
        {
            return Ok((await _service.TabCloseGetAll()));
        }
        

        [HttpPost]
        public async Task<ActionResult> Accept([FromForm] IdeaDto model)
        {
            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

           

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];
                

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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Accept(model);
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Reject([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

              

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Reject(model);
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Satisfied([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

              

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Satisfied(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Dissatisfied([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

                

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Dissatisfied(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }


        [HttpPost]
        public async Task<ActionResult> Assign([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

              

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Update(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Close([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

               

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Close(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

              

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Update(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Complete([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];


                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Complete(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Terminate([FromForm] IdeaDto model)
        {

            if (ModelState.IsValid)
            {
                var fileupload = new List<Files>();

                var file = Request.Form.Files["UploadedFile"];

               

                var ideaId = Request.Form["IdeaID"];
                var inserBy = Request.Form["CreatedBy"];
                var comment = Request.Form["Comment"];


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
                        var datas = new Files
                        {
                            Path = $"/UploadedFiles/{currentFile.FileName}"
                        };
                        fileupload.Add(datas);
                    }
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }
                else
                {
                    model.IdeaId = ideaId.ToInt();
                    model.Comment = comment;
                    model.CreatedBy = inserBy.ToInt();
                    model.File = fileupload;
                }


                var data = await _service.Terminate(model);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", data, "message");
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

       

        

    }
}
