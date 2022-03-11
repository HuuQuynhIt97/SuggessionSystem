using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Suggession.Helpers;
using Suggession.Constants;
using Microsoft.AspNetCore.Http;
using NetUtility;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Suggession._Repositories.Repositories;
using Suggession._Repositories.Interface;

namespace Suggession._Repositories.Repositories
{
    public class PlanIdeaRepository : RepositoryBase<PlanIdea>, IPlanIdeaRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PlanIdeaRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }

}
