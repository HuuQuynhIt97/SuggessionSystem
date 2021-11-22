using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession._Repositories.Repositories;

namespace Suggession._Repositories.Interface
{
    public class TabRepository : RepositoryBase<Tab>, ITabRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TabRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }



}
