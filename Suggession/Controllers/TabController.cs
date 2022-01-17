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

namespace Suggession.Controllers
{
    public class TabController : ApiControllerBase
    {
        private readonly ITabService _service;

        public TabController(ITabService service)
        {
            _service = service;
        }

        [HttpGet("{lang}")]
        public async Task<ActionResult> GetAllAsync(string lang)
        {
            return Ok(await _service.GetAll(lang));
        }

    }
}
