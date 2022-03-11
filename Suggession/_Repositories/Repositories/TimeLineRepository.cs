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
using Suggession._Repositories.Interface;

namespace Suggession._Repositories.Repositories
{
    public class TimeLineRepository : RepositoryBase<TimeLine>, ITimeLineRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TimeLineRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }



}
