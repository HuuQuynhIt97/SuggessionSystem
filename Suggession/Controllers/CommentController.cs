using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession._Services.Services;
using System.Threading.Tasks;
using Suggession._Services.Interface;

namespace Suggession.Controllers
{
    public class CommentController : ApiControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService service)
        {
            _service = service;
        }
        
     
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CommentDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

        
    }
}
