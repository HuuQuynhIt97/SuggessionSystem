using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession._Services.Interface;
using Suggession.Models;

namespace Suggession.Controllers
{
    public class PlanIdeaController : ApiControllerBase
    {
        private readonly IPlanIdeaService _service;

        public PlanIdeaController(IPlanIdeaService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPlanIdea(int id)
        {
            return Ok(await _service.GetPlanIdea(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPlanOkIdea(int id)
        {
            return Ok(await _service.GetPlanOkIdea(id));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> SubmitPlanIdea(int id)
        {
            return Ok(await _service.SubmitPlanIdea(id));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFactoryHeadComment(int id)
        {
            return Ok(await _service.GetFactoryHeadComment(id));
        }

        [HttpGet]
        public async Task<ActionResult> GetPlanStatus()
        {
            return Ok(await _service.GetPlanStatus());
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<PlanIdea> create)
        {

            return Ok(await _service.Add(create));
            throw new Exception("Creating failed on save");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(List<PlanIdea> create)
        {
            return Ok(await _service.CreateOrUpdate(create));
            throw new Exception("Creating failed on save");
        }



        [HttpPut]
        public async Task<IActionResult> Update(List<PlanIdea> update)
        {
            return Ok(await _service.Update(update));
            
            return BadRequest($"Updating failed on save");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
            throw new Exception("Error deleting the Version");
        }

    }
}
