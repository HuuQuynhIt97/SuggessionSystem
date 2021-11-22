using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Suggession.Constants;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Suggession._Services.Services
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAllAsync();
        Task<OperationResult> LockAsync(int id);
        Task<OperationResult> AddAsync(AccountDto model);
        Task<OperationResult> UpdateAsync(AccountDto model);
        Task<OperationResult> DeleteAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<object> GetAccounts();
    }
    
}
