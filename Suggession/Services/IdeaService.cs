using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession.Services.Base;
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

namespace Suggession.Services
{
    public interface IIdeaService : IServiceBase<Idea, IdeaDto>
    {
        //Task<object> GetKPIByOcID(int ocID);
        //Task<object> GetPolicyByOcID(int ocID);
        Task<object> TabProposalGetAll();
        Task<object> GetIdeaHisById(int id);
        Task<object> TabProcessingGetAll();
        Task<bool> UploadFile(IdeaDto entity);
        Task<object> TabErickGetAll();
        Task<object> TabCloseGetAll();
        Task<bool> Delete(int id);
        Task<bool> Accept(IdeaDto entity);
        Task<bool> Reject(IdeaDto entity);
        Task<bool> Update(IdeaDto entity);
        Task<bool> Close(IdeaDto entity);
        Task<bool> Complete(IdeaDto entity);
        Task<bool> Terminate(IdeaDto entity);
        Task<bool> Satisfied(IdeaDto entity);
        Task<bool> Dissatisfied(IdeaDto entity);
    }
    public class IdeaService : ServiceBase<Idea, IdeaDto>, IIdeaService
    {
        private readonly IRepositoryBase<Idea> _repo;
        private readonly IRepositoryBase<IdeaHistory> _repoIdeaHis;
        private readonly IRepositoryBase<UploadFile> _repoUp;
        private readonly IRepositoryBase<Models.Status> _repoStatus;
        private readonly IRepositoryBase<OCPolicy> _repoOcPolicy;
        private readonly IRepositoryBase<OC> _repoOc;
        private readonly IRepositoryBase<Types> _repoType;
        private readonly IRepositoryBase<Account> _repoAc;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public IdeaService(
            IRepositoryBase<Idea> repo,
            IRepositoryBase<UploadFile> repoUp,
            IRepositoryBase<IdeaHistory> repoIdeaHis,
            IRepositoryBase<Models.Status> repoStatus,
            IRepositoryBase<Types> repoType,
            IRepositoryBase<Account> repoAc,
            IRepositoryBase<OCPolicy> repoOcPolicy,
            IRepositoryBase<OC> repoOc,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoUp = repoUp;
            _repoIdeaHis = repoIdeaHis;
            _repoStatus = repoStatus;
            _repoOcPolicy = repoOcPolicy;
            _repoOc = repoOc;
            _repoType = repoType;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<bool> UploadFile(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var idea = _mapper.Map<Idea>(entity);
            _repo.Add(idea);
            await _unitOfWork.SaveChangeAsync();

            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = idea.Id;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = entity.Status;
            ideaHist.Isshow = true;
            ideaHist.Comment = "Issue: " + entity.Issue + "\n" + "Suggession: " + entity.Suggession;
            _repoIdeaHis.Add(ideaHist);

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<object> TabProposalGetAll()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var data = await _repo.FindAll(
            //    x => x.CreatedBy == accountId || x.ReceiveID == accountId)
            //.Where(x =>
            //x.Status == Suggession.Constants.Status.Apply ||
            //x.Status == Suggession.Constants.Status.NA ||
            //x.Status == Suggession.Constants.Status.Reject ||
            //x.Status == Suggession.Constants.Status.Terminate ||
            //x.Status == Suggession.Constants.Status.Complete
            ).Select(x => new IdeaDto {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Issue = x.Issue,
                Suggession = x.Suggession,
                Title = x.Title,
                StatusName = _repoStatus.FindById(x.Status).Name ?? "N/A"

            }).ToListAsync();
            return data;
        }

        public async Task<object> TabProcessingGetAll()
        {
            var data = await _repo.FindAll(x => x.Status == Suggession.Constants.Status.Update).Select(x => new IdeaDto
            {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Issue = x.Issue,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Suggession = x.Suggession,
                Title = x.Title,
                StatusName = _repoStatus.FindById(x.Status).Name ?? ""

            }).ToListAsync();
            return data;
        }

        public async Task<object> TabErickGetAll()
        {
            var data = await _repo.FindAll(x => x.Status == Suggession.Constants.Status.Dissatisfied).Select(x => new IdeaDto
            {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Issue = x.Issue,
                Suggession = x.Suggession,
                Title = x.Title,
                StatusName = _repoStatus.FindById(x.Status).Name ?? ""

            }).ToListAsync();
            return data;
        }

        public async Task<object> TabCloseGetAll()
        {
            var data = await _repo.FindAll(x =>
            x.Status == Suggession.Constants.Status.Satisfied ||
            x.Status == Suggession.Constants.Status.Close).Select(x => new IdeaDto
            {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Issue = x.Issue,
                Suggession = x.Suggession,
                Title = x.Title,
                StatusName = _repoStatus.FindById(x.Status).Name ?? ""

            }).ToListAsync();
            return data;
        }

        public async Task<object> GetIdeaHisById(int id)
        {
            var data = await _repoIdeaHis.FindAll(x => x.IdeaID == id).Select(x => new IdeaDto { 
                Id = x.Id,
                IdeaId = x.IdeaID,
                Comment = x.Comment,
                Name = _repoAc.FindById(x.InsertBy).FullName ?? "",
                CreatedTime = x.CreatedTime,
                StatusName = _repoStatus.FindById(x.Status).Name ?? ""
            }).ToListAsync();
            return data;
        }

        public async Task<bool> Accept(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Update;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Update;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Reject(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Reject;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Reject;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Close(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Close;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Close;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Update(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Update;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Update;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Complete(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Complete;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Complete;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }
        public async Task<bool> Terminate(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Terminate;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Terminate;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }
        public async Task<bool> Satisfied(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Satisfied;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Satisfied;

            await _unitOfWork.SaveChangeAsync();

            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> Dissatisfied(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.Dissatisfied;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _unitOfWork.SaveChangeAsync();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Dissatisfied;

            await _unitOfWork.SaveChangeAsync();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = ideaHist.Id
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
