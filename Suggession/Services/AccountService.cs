using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public interface IAccountService : IServiceBase<Account, AccountDto>
    {
        Task<OperationResult> LockAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<object> GetAccounts();
    }
    public class AccountService : ServiceBase<Account, AccountDto>, IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IRepositoryBase<OC> _repoOc;
        private readonly IRepositoryBase<AccountGroupAccount> _repoAccountGroupAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;

        public AccountService(
            IRepositoryBase<Account> repo,
            IRepositoryBase<OC> repoOC,
            IRepositoryBase<AccountGroupAccount> repoAccountGroupAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoOc = repoOC;
            _repoAccountGroupAccount = repoAccountGroupAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(AccountDto model)
        {
            try
            {
                var item = _mapper.Map<Account>(model);
                item.Password = item.Password.ToEncrypt();
                _repo.Add(item);

                await _unitOfWork.SaveChangeAsync();
                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, item.Id));
                }
                _repoAccountGroupAccount.AddRange(list);
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
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(AccountDto model)
        {
            try
            {
                var item = await _repo.FindByIdAsync(model.Id);
                if (model.Password.IsBase64() == false)
                        item.Password = model.Password.ToEncrypt();
                item.Username = model.Username;
                item.FullName = model.FullName;
                item.Email = model.Email;
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                var removingList = await _repoAccountGroupAccount.FindAll(x => x.AccountId == item.Id).ToListAsync();
                _repoAccountGroupAccount.RemoveMultiple(removingList);
                await _unitOfWork.SaveChangeAsync();

                var list = new List<AccountGroupAccount>();
                foreach (var accountGroupId in model.AccountGroupIds)
                {
                    list.Add(new AccountGroupAccount(accountGroupId, item.Id));
                }
                _repoAccountGroupAccount.AddRange(list);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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
        public override async Task<List<AccountDto>> GetAllAsync()
        {
            var query = _repo.FindAll().Select(x => new AccountDto {
                Id = x.Id,
                Username = x.Username,
                Password = x.Password,
                CreatedBy = x.CreatedBy,
                CreatedTime = x.CreatedTime,
                ModifiedBy = x.ModifiedBy,
                ModifiedTime = x.ModifiedTime,
                IsLock = x.IsLock,
                AccountGroupIds = x.AccountGroupAccount.Count > 0 ? x.AccountGroupAccount.Select(x => x.AccountGroup.Id).ToList() : new List<int> { },
                AccountGroupText = x.AccountGroupAccount.Count > 0 ? String.Join(",", x.AccountGroupAccount.Select(x => x.AccountGroup.Name)) : "",
                FullName = x.FullName,
                Email = x.Email,
            });
            var data = await query.ToListAsync();
            return data;

        }


        public async Task<OperationResult> LockAsync(int id)
        {
            var item = await _repo.FindByIdAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            item.IsLock = !item.IsLock;
            try
            {
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = item.IsLock ? MessageReponse.LockSuccess : MessageReponse.UnlockSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<AccountDto> GetByUsername(string username)
        {
            var result = await _repo.FindAll(x => x.Username.ToLower() == username.ToLower()).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
            return result;
        }

       public async Task<object> GetAccounts()
        {
            var query = await _repo.FindAll().Select(x=> new { 
            x.Username,
            x.Id,
            x.FullName,
            IsLeader = x.AccountGroupAccount.Any(a => a.AccountGroup.Position  == SystemRole.FunctionalLeader)
            }).ToListAsync();
            return query;
        }
    }
}
