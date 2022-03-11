using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Suggession.Helpers;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Suggession._Services.Interface
{
    public interface IStatusService
    {
        Task<List<StatusDto>> GetAllAsync();
        Task<OperationResult> AddAsync(StatusDto model);
        Task<OperationResult> UpdateAsync(StatusDto model);
       Task<OperationResult> DeleteAsync(int id);

    }
    
}
