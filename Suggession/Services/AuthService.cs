using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;

namespace Suggession.Services
{
    public interface IAuthService
    {
        Task<Account> Login(string username, string password);
        Task<bool> CheckLock(string username);
    }
    public class AuthService : IAuthService
    {
        private readonly IRepositoryBase<Account> _repo;

        public AuthService(
            IRepositoryBase<Account> repo
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
