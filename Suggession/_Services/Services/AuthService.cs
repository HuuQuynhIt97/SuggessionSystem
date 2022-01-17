using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;
using Suggession._Repositories.Interface;
using Suggession._Services.Interface;

namespace Suggession._Services.Services
{
   
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _repo;

        public AuthService(
            IAccountRepository repo
            )
        {
            _repo = repo;
        }

        public async Task<bool> CheckLock(string username)
        {
            var account = await _repo.FindAll().FirstOrDefaultAsync(x =>x.Username == username);

            if (account == null)
                return false;
            
            return account.IsLock;

        }
        public async Task<Account> LoginAnonymous(string username)
        {
            var account = await _repo.FindAll(x => x.Username == username).FirstOrDefaultAsync();
            if (account == null)
                return null;
            return account;

        }
        public async Task<Account> Login(string username, string password)
        {
            var account = await _repo.FindAll().FirstOrDefaultAsync(x => x.Username == username);

            if (account == null)
                return null;
            if (account.Password.ToDecrypt() == password)
                return account;
            return null;

        }

    }
}
