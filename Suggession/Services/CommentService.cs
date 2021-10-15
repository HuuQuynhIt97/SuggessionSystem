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
    public interface ICommentService: IServiceBase<Comment, CommentDto>
    {
        Task<List<CommentDto>> GetAllByObjectiveId(int objectiveId);
        Task<CommentDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType);
        Task<object> GetFunctionalLeaderCommentByAccountId(int accountId, int periodTypeId, int period);
        Task<object> GetL1CommentByAccountId(int accountId, int periodTypeId, int period);
        Task<object> GetGHRCommentByAccountId(int accountId, int periodTypeId, int period);
        Task<object> GetL0SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period);
        Task<object> GetL1SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period);
        Task<object> GetL2SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period);

    }
    public class CommentService : ServiceBase<Comment, CommentDto>, ICommentService
    {
        private OperationResult operationResult;

        private readonly IRepositoryBase<Comment> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        public CommentService(
            IRepositoryBase<Comment> repo, 
            IRepositoryBase<Account> repoAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper,  configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
        }
       
        public async Task<CommentDto> GetFisrtByAccountId(int accountId, int periodTypeId, int period, string scoreType)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CommentDto>> GetAllByObjectiveId(int objectiveId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Chỉnh sửa thành vừa cập nhật vừa thêm mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(CommentDto model)
        {
            if (model.Id > 0)
            {
                var item = await _repo.FindAll(x => x.Id == model.Id && x.CreatedBy == model.CreatedBy).AsNoTracking().FirstOrDefaultAsync();
                item.Content = model.Content;
                _repo.Update(item);
            }
            else
            {
                var itemList = _mapper.Map<Comment>(model);
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

        public  async Task<object> GetFunctionalLeaderCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }
       
        public async Task<object> GetGHRCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetL1CommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetL0SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }
        public async Task<object> GetL1SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }
        public async Task<object> GetL2SelfEvaluationCommentByAccountId(int accountId, int periodTypeId, int period)
        {
            throw new NotImplementedException();
        }
    }
}
