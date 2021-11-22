using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession._Services.Services
{
    public interface ITabService
    {
        Task<object> GetAll();
    }
    

   
}
