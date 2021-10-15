using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Helpers
{
    public static class DBInitializer
    {
        //private readonly DataContext _context;
        //public DBInitializer(DataContext context)
        //{
        //    _context = context;
        //}
        public static void Seed(DataContext _context)
        {
           


            #region Tài Khoản
            //if (!(_context.Accounts.Any()))
            //{
            //    var supper = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("SYSTEM"));
            //    var user = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("MEMBER"));
            //    var account1 = new Account { Username = "admin", Password = "1", AccountTypeId = supper.Id };
            //    var account2 = new Account { Username = "user", Password = "1", AccountTypeId = user.Id };
            //    _context.Accounts.AddRange(new List<Account> {account1,
            //       account2
            //    });
            //    _context.SaveChanges();
            //}

            #endregion

            #region Nhóm Tài Khoản
            if (!(_context.AccountGroups.Any()))
            {
                _context.AccountGroups.AddRange(new List<AccountGroup> {
                    new AccountGroup { Name = "L0", Position = 1 },
                    new AccountGroup { Name = "L1", Position = 2 },
                    new AccountGroup { Name = "L2", Position = 3 },
                    new AccountGroup { Name = "FHO", Position = 4 },
                    new AccountGroup { Name = "GHM", Position = 5 },
                    new AccountGroup { Name = "GM", Position = 6 }
            });
                _context.SaveChanges();
            }

            #endregion


        }
    }
}
