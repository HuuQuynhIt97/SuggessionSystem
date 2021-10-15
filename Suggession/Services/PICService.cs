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
    public interface IPICService: IServiceBase<PIC, PICDto>
    {
    }
    public class PICService : ServiceBase<PIC, PICDto>, IPICService
    {
        private readonly IRepositoryBase<PIC> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public PICService(
            IRepositoryBase<PIC> repo, 
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
