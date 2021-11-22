using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;

namespace Suggession._Services.Services
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
        Task<bool> CheckLock(string username);
    }
    
}
