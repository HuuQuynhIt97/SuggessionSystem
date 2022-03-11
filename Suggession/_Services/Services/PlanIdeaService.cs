using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession._Repositories.Interface;
using Suggession.Constants;
using Suggession._Services.Interface;
using Suggession.Helpers;
using Microsoft.AspNetCore.Http;

namespace Suggession._Services.Services
{
    
    public class PlanIdeaService : IPlanIdeaService
    {
        private readonly IPlanIdeaRepository _repo;
        private readonly IPlanStatusRepository _repoPlanStatus;
        private readonly IAccountRepository _repoAc;
        private readonly IAccountGroupAccountRepository _repoAcGroupAc;
        private readonly IIdeaHistoryRepository _repoIdeaHis;
        private readonly IIdeaRepository _repoIdea;
        private readonly IAccountGroupRepository _repoAcGroup;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PlanIdeaService(
            IPlanIdeaRepository repo,
            IPlanStatusRepository repoPlanStatus,
            IAccountRepository repoAc,
            IIdeaHistoryRepository repoIdeaHis,
            IIdeaRepository repoIdea,
            IAccountGroupAccountRepository repoAcGroupAc,
            IAccountGroupRepository repoAcGroup,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoPlanStatus = repoPlanStatus;
            _repoAc = repoAc;
            _repoIdeaHis = repoIdeaHis;
            _repoIdea = repoIdea;
            _repoAcGroupAc = repoAcGroupAc;
            _repoAcGroup = repoAcGroup;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<List<PlanStatus>> GetPlanStatus()
        {
            var data = _repoPlanStatus.FindAll().ToList();
            return data;
        }
        public async Task<object> GetFactoryHeadComment(int id)
        {
            var userFactoryHead = _repoAcGroupAc.FindAll(x => x.AccountGroupId == SystemAccountGroup.FactoryHead).FirstOrDefault().AccountId;
            var userCommentFactoryHead = from x in _repoIdeaHis.FindAll(x => x.IdeaID == id && x.InsertBy == userFactoryHead)
                                         join y in _repoAc.FindAll() on x.InsertBy equals y.Id
                                         select new
                                         {
                                             Name = y.FullName,
                                             Status = x.Status,
                                             Comment = x.Comment,
                                             TimeSort = x.CreatedTime,
                                             Time = x.CreatedTime.ToString("yyyy-MM-dd")
                                         };
            return userCommentFactoryHead.Where(x => !String.IsNullOrEmpty(x.Comment) && x.Status == Suggession.Constants.Status.ApproveStatus).OrderByDescending(x => x.TimeSort);
        }

        public async Task<List<PlanIdeaDto>> GetPlanIdea(int id)
        {
            var index = 1;
            var data = (from x in _repo.FindAll(x => x.IdeaID == id)
                       join y in _repoPlanStatus.FindAll() on x.StatusPlanID equals y.ID
                select new PlanIdeaDto { 
                ID = x.ID,
                IdeaID = x.IdeaID,
                Plan = x.Plan,
                Index = index,
                CreatedBy = x.CreatedBy,
                IsDisplay = x.IsDisplay,
                CreatedTime = x.CreatedTime,
                Description = x.Description,
                PlanStatusName = y.Name,
                StatusPlanID = x.StatusPlanID
            }).Where(x => x.IsDisplay).OrderByDescending(x => x.CreatedTime).ToList();

            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<List<PlanIdeaDto>> GetPlanOkIdea(int id)
        {
            var index = 1;
            var data = (from x in _repo.FindAll(x => x.IdeaID == id)
                        join y in _repoPlanStatus.FindAll() on x.StatusPlanID equals y.ID
                        select new PlanIdeaDto
                        {
                            ID = x.ID,
                            IdeaID = x.IdeaID,
                            Plan = x.Plan,
                            Index = index,
                            CreatedBy = x.CreatedBy,
                            IsDisplay = x.IsDisplay,
                            CreatedTime = x.CreatedTime,
                            Description = x.Description,
                            PlanStatusName = y.Name,
                            StatusPlanID = x.StatusPlanID
                        }).Where(x => x.IsDisplay && x.StatusPlanID == Constants.Status.OK).OrderByDescending(x => x.CreatedTime).ToList();

            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<bool> Add(List<PlanIdea> model)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            model.ForEach(item =>
            {
                item.CreatedBy = accountId;
                item.CreatedTime = DateTime.Now;
            });
            try
            {
                _repo.AddRange(model);
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }

        public async Task<bool> Update(List<PlanIdea> model)
        {
            
            try
            {
                _repo.UpdateRange(model);
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = _repo.FindById(id);
                item.IsDisplay = false;
                item.DeleteTime = DateTime.Now;
                //_repo.Remove(item);
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
        }

        public async Task<bool> CreateOrUpdate(List<PlanIdea> model)
        {
            var listPlanAdd = model.Where(x => x.ID == 0).ToList();
            if (listPlanAdd.Count > 0)
            {
                model.ForEach(item =>
                {
                    item.CreatedTime = DateTime.Now;
                });
            }
            var listPlanUpdate = model.Where(x => x.ID > 0).ToList();

            try
            {
                var addPlan = _mapper.Map<List<PlanIdea>>(listPlanAdd);
                var updatePlan = _mapper.Map<List<PlanIdea>>(listPlanUpdate);

                _repo.AddRange(addPlan);
                _repo.UpdateRange(updatePlan);
                await _repo.SaveAll();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> SubmitPlanIdea(int id)
        {

            try
            {
                var item = _repoIdea.FindById(id);
                item.IsShowApproveTab = true;
                _repoIdea.Update(item);
                await _repoIdea.SaveAll();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
            throw new NotImplementedException();
        }
    }

   
}
