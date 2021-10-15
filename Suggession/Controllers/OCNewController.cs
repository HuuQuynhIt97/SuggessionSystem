﻿using Microsoft.AspNetCore.Http;
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
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OCNewController : ApiControllerBase
    {
        private readonly IOCNewService _service;

        public OCNewController(IOCNewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllLevel3()
        {
            var oc = await _service.GetAllLevel3();
            return Ok(oc);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsTreeView()
        {
            var ocs = await _service.GetAllAsTreeView();
            return Ok(ocs);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] OCNewDto model)
        {
            return StatusCodeResult(await _service.AddAsync(model));
        }

       
        [HttpGet("{ocID}")]
        public async Task<IActionResult> GetUserByOcID(int ocID)
        {
            var result = await _service.GetUserByOcID(ocID);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> MappingUserOC([FromBody]OCAccountDto OcUserDto)
        {
            var result = await _service.MappingUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MappingRangeUserOC(OCAccountDto OcUserDto)
        {
            var result = await _service.MappingRangeUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserOC([FromBody]OCAccountDto OcUserDto)
        {
            var result = await _service.RemoveUserOC(OcUserDto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] OCNewDto model)
        {
            return StatusCodeResult(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
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

        

    }
}