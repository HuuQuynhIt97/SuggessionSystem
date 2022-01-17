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
    public interface IAccountGroupService
    {
        Task<List<AccountGroupDto>> GetAccountGroupForTodolistByAccountId();

        Task<List<AccountGroupDto>> GetAllAsync();
        Task<OperationResult> AddAsync(AccountGroupDto model);
        Task<OperationResult> UpdateAsync(AccountGroupDto model);
       Task<OperationResult> DeleteAsync(int id);

    }
    
}
