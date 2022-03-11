using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Suggession.Helpers;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Suggession._Repositories.Interface;
using System.Net;
using Suggession.Constants;
using Suggession._Services.Interface;

namespace Suggession._Services.Services
{
   
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repo;
        private readonly IAccountGroupAccountRepository _repoAccount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public StatusService(
            IStatusRepository repo,
            IAccountGroupAccountRepository repoAccount,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        
        public async Task<List<StatusDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<StatusDto>(_configMapper).ToListAsync();
        }

        public async Task<OperationResult> AddAsync(StatusDto model)
        {
            try
            {
                var item = _mapper.Map<Models.Status>(model);
                _repo.Add(item);
                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item.Id
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateAsync(StatusDto model)
        {
            try
            {
                var item = _mapper.Map<Models.Status>(model);
                _repo.Update(item);
                await _repo.SaveAll();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item.Id
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var delete = _repo.FindById(id);
            _repo.Remove(delete);

            try
            {
                await _repo.SaveAll();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = delete
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
    }
}
