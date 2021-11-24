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
using Suggession._Repositories.Interface;

namespace Suggession._Services.Services
{

    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _repo;
        private readonly IIdeaHistoryRepository _repoIdeaHis;
        private readonly IUploadFileRepository _repoUp;
        private readonly IStatusRepository _repoStatus;
        private readonly IAccountRepository _repoAc;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public IdeaService(
            IIdeaRepository repo,
            IUploadFileRepository repoUp,
            IIdeaHistoryRepository repoIdeaHis,
            IStatusRepository repoStatus,
            IAccountRepository repoAc,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoUp = repoUp;
            _repoIdeaHis = repoIdeaHis;
            _repoStatus = repoStatus;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<bool> UploadFile(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var idea = _mapper.Map<Idea>(entity);
            _repo.Add(idea);
            await _repo.SaveAll();

            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = idea.Id;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = entity.Status;
            ideaHist.Isshow = true;
            ideaHist.Comment = "Issue: " + entity.Issue + "\n" + "Suggession: " + entity.Suggession;
            _repoIdeaHis.Add(ideaHist);

            await _repoIdeaHis.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public async Task<bool> EditSuggession(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var itemID = _repoIdeaHis.FindAll(x => x.IdeaID == entity.Id).FirstOrDefault().Id;
            var itemUpdate = _repo.FindById(entity.Id);
            itemUpdate.Status = entity.Status;
            itemUpdate.SendID = entity.SendID;
            itemUpdate.ReceiveID = entity.ReceiveID;
            itemUpdate.Title = entity.Title;
            itemUpdate.Issue = entity.Issue;
            itemUpdate.Suggession = entity.Suggession;
            itemUpdate.CreatedBy = entity.CreatedBy;

            _repo.Update(itemUpdate);
            await _repo.SaveAll();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = itemID
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<bool> EditSubmitSuggession(IdeaDto entity)
        {
            var ListUpload = new List<UploadFile>();
            var itemID = _repoIdeaHis.FindAll(x => x.IdeaID == entity.Id).FirstOrDefault().Id;
            var itemUpdate = _repo.FindById(entity.Id);
            itemUpdate.Status = entity.Status;
            itemUpdate.SendID = entity.SendID;
            itemUpdate.ReceiveID = entity.ReceiveID;
            itemUpdate.Title = entity.Title;
            itemUpdate.Issue = entity.Issue;
            itemUpdate.Suggession = entity.Suggession;
            itemUpdate.CreatedBy = entity.CreatedBy;
            _repo.Update(itemUpdate);
            await _repo.SaveAll();
            foreach (var item in entity.File)
            {
                ListUpload.Add(new UploadFile
                {
                    Path = item.Path,
                    IdealID = itemID
                });
            }
            try
            {
                _repoUp.AddRange(ListUpload);
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
                StatusName = _repoStatus.FindById(x.Status).Name ?? "N/A"
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Update;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Reject;
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Close;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Update;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Complete;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Terminate;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Satisfied;

            await _repo.SaveAll();

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
                await _repoUp.SaveAll();
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw;
            }
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
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.Status = Suggession.Constants.Status.Dissatisfied;

            await _repo.SaveAll();
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
                await _repoUp.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        
    }
}
