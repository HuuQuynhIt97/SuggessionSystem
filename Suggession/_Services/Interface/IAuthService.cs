using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;

namespace Suggession._Services.Interface
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
        Task<Account> LoginAnonymous(string username);
        Task<bool> CheckLock(string username);
    }
    
}
