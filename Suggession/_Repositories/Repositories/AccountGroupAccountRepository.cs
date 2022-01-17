using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession._Repositories.Repositories;
using Suggession._Repositories.Interface;

namespace Suggession._Repositories.Repositories
{
    public class AccountGroupAccountRepository : RepositoryBase<AccountGroupAccount>, IAccountGroupAccountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AccountGroupAccountRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }

}
