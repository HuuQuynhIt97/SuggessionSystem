using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NetUtility;

namespace Suggession.Controllers
{
    public class IdeaController : ApiControllerBase
    {
        private readonly IIdeaService _service;
        public readonly IWebHostEnvironment _environment;
        public IdeaController(IIdeaService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
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
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok((await _service.GetAllAsync()));
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] IdeaDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
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
                return Ok(data);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return Ok(model);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] IdeaDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteAsync(int id)
        //{
        //    return Ok(await _service.Delete(id));
        //}

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

    }
}
