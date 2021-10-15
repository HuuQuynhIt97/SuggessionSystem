using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Suggession.Constants;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Helpers;
using Suggession.Models;
using Suggession.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Suggession.Services
{
    public interface IScoreService : IServiceBase<Score, ScoreDto>
    {
        Task<ScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        Task<ScoreDto> GetFisrtScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        Task<ScoreDto> GetFisrtSelfScoreByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        Task<ScoreDto> GetFisrtSelfScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType);
    }
    public class ScoreService : ServiceBase<Score, ScoreDto>, IScoreService
    {
        private readonly IRepositoryBase<Score> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<PeriodType> _repoPeriodType;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public ScoreService(
            IRepositoryBase<Score> repo,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<PeriodType> repoPeriodType,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _repoPeriodType = repoPeriodType;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }
        public async Task<ScoreDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int scoreBy = JWTExtensions.GetDecodeTokenById(accessToken);

            return await _repo.FindAll(x => 
                                        x.ScoreType == scoreType 
                                        && x.PeriodTypeId == periodTypeId 
                                        && x.CreatedTime.Year == DateTime.Today.Year 
                                        && x.Period == period
                                        && accountId == x.AccountId 
                                        && scoreBy == x.ScoreBy 
                                        && x.AccountId != scoreBy)
                                    .ProjectTo<ScoreDto>(_configMapper)
                                    .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(ScoreDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.ScoreBy == model.ScoreBy).AsNoTracking().FirstOrDefaultAsync();
                item.Point = model.Point;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<Score>(model);
                _repo.Add(itemList);
            }
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<ScoreDto> GetFisrtSelfScoreByAccountId(int scoreBy, int periodTypeId, int period, string scoreType)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenById(accessToken);
         
            // tu cham cho minh
            return await _repo.FindAll(x =>  
                                    x.PeriodTypeId == periodTypeId 
                                    && x.CreatedTime.Year == DateTime.Today.Year 
                                    && x.Period == period 
                                    && x.ScoreBy == scoreBy
                                    && x.ScoreType == scoreType
                                    && x.AccountId == scoreBy
                                    && x.AccountId == accountID
                                ).ProjectTo<ScoreDto>(_configMapper).FirstOrDefaultAsync();
        }

        public async Task<ScoreDto> GetFisrtSelfScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            return await _repo.FindAll(x =>
                                    x.PeriodTypeId == periodTypeId
                                    && x.CreatedTime.Year == DateTime.Today.Year
                                    && x.Period == period
                                    && x.ScoreBy == accountId
                                    && x.ScoreType == ScoreType.L0
                                    && x.AccountId == accountId
                                ).ProjectTo<ScoreDto>(_configMapper).FirstOrDefaultAsync();
        }

        public async Task<ScoreDto> GetFisrtScoreL1ByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            throw new NotImplementedException();
        }
    }
}
