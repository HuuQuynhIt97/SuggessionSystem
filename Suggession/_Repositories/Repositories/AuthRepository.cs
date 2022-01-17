using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;
using Suggession._Repositories.Repositories;
using AutoMapper;
using Suggession._Repositories.Interface;

namespace Suggession._Repositories.Repositories
{
    public class AuthRepository : RepositoryBase<Account>, IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
