﻿using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Services;
using System.Threading.Tasks;
using System.Linq;

namespace Suggession.Controllers
{
    public class KPINewController : ApiControllerBase
    {
        private readonly IKPINewService _service;

        public KPINewController(IKPINewService service)
        {
            _service = service;
        }
        [HttpGet("{ocID}")]
        public async Task<IActionResult> GetKPIByOcID(int ocID)
        {
            var result = await _service.GetKPIByOcID(ocID);
            return Ok(result);
        }
        [HttpGet("{ocID}")]
        public async Task<IActionResult> GetPolicyByOcID(int ocID)
        {
            var result = await _service.GetPolicyByOcID(ocID);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllType()
        {
            return Ok((await _service.GetAllType()));
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok((await _service.GetAllAsync()));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsTreeView()
        {
            var ocs = await _service.GetAllAsTreeView();
            return Ok(ocs);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] KPINewDto model)
        {

            return StatusCodeResult(await _service.AddAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] KPINewDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return Ok(await _service.Delete(id));
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

    }
}
