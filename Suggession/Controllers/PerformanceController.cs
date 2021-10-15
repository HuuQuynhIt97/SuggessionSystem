using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Controllers
{
    public class PerformanceController : ApiControllerBase
    {
        private readonly IPerformanceService _service;

        public PerformanceController(IPerformanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetKPIObjectivesByUpdater()
        {
            return Ok(await _service.GetKPIObjectivesByUpdater());
        }
       
        [HttpPut]
        public async Task<ActionResult> Submit([FromBody] List<PerformanceDto> model)
        {
            return StatusCodeResult(await _service.Submit(model));
        }


    }
}
