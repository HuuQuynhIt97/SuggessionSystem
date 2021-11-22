using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Controllers
{
    public class AccountGroupAccountController : ApiControllerBase
    {
        private readonly IAccountGroupAccountService _service;

        public AccountGroupAccountController(IAccountGroupAccountService service)
        {
            _service = service;
        }

        

    }
}
