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
    public interface IPeriodReportTimeService: IServiceBase<PeriodReportTime, PeriodReportTimeDto>
    {
    }
    public class PeriodReportTimeService : ServiceBase<PeriodReportTime, PeriodReportTimeDto>, IPeriodReportTimeService
    {
        private readonly IRepositoryBase<PeriodReportTime> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public PeriodReportTimeService(
            IRepositoryBase<PeriodReportTime> repo, 
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
