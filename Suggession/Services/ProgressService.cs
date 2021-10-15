﻿using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Services
{
    public interface IProgressService: IServiceBase<Progress, ProgressDto>
    {
    }
    public class ProgressService : ServiceBase<Progress, ProgressDto>, IProgressService
    {
        private readonly IRepositoryBase<Progress> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ProgressService(
            IRepositoryBase<Progress> repo, 
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
    }
}
