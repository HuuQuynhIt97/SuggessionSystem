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
using Suggession._Services.Interface;

namespace Suggession._Services.Services
{

    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _repo;
        private readonly IIdeaHistoryRepository _repoIdeaHis;
        private readonly ITimeLineRepository _repoTimeLine;
        private readonly IPlanIdeaRepository _repoPlanIdea;
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
            ITimeLineRepository repoTimeLine,
            IPlanIdeaRepository repoPlanIdea,
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
            _repoTimeLine = repoTimeLine;
            _repoIdeaHis = repoIdeaHis;
            _repoPlanIdea = repoPlanIdea;
            _repoStatus = repoStatus;
            _httpContextAccessor = httpContextAccessor;
            _repoAc = repoAc;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<bool> UpdateAnnouncement(int id)
        {
            try
            {
                var item = _repo.FindById(id);
                item.IsAnnouncement = !item.IsAnnouncement;
                _repo.Update(item);
                await _repo.SaveAll();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
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
            ideaHist.Comment = "Issue: " + entity.Issue + "\n" + "Suggestion: " + entity.Suggession;
            ideaHist.CommentZh = "問題: " + entity.Issue + "\n" + "建議: " + entity.Suggession;
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
            catch (Exception)
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
            catch (Exception)
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
            var ideaHis = _repoIdeaHis.FindAll(x => x.IdeaID == entity.Id).FirstOrDefault();
            ideaHis.Status = entity.Status;
            _repoIdeaHis.Update(ideaHis);
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
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<object> TabProposalGetAll(string lang)
        {
            var index = 1;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountId = JWTExtensions.GetDecodeTokenById(accessToken);
            var data = (from x in await _repo.FindAll(x =>
            x.Status == Suggession.Constants.Status.Apply ||
            x.Status == Suggession.Constants.Status.Reject ||
            x.Status == Suggession.Constants.Status.Complete ||
            x.Status == Suggession.Constants.Status.Terminate).ToListAsync()
                        join y in _repoIdeaHis.FindAll() on x.Id equals y.IdeaID into last
                        let l = last.LastOrDefault()
                        select new IdeaDto {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Index = index,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Issue = x.Issue,
                Suggession = x.Suggession,
                Title = x.Title,
                Comment= l.Comment,
                Isshow = x.Isshow,
                IsAnnouncement = x.IsAnnouncement,
                Description = _repoStatus.FindById(l.Status) != null ? _repoStatus.FindById(l.Status).Description : "N/A",
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status) != null ? _repoStatus.FindById(x.Status).NameEn : "N/A" : _repoStatus.FindById(x.Status) != null ? _repoStatus.FindById(x.Status).NameZh : "N/A",
                Type = _repoStatus.FindById(x.Status) != null ? _repoStatus.FindById(x.Status).Description : "N/A"

            }).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> TabProcessingGetAll(string lang)
        {
            int index = 1;
            var data =  (from x in await _repo.FindAll(x => x.Status == Suggession.Constants.Status.Update).ToListAsync()
                        join y in _repoIdeaHis.FindAll() on x.Id equals y.IdeaID into last
                        let l = last.LastOrDefault()
                select new IdeaDto
            {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Issue = x.Issue,
                Index = index,
                Comment = l.Comment,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Suggession = x.Suggession,
                Isshow = x.Isshow,
                IsAnnouncement = x.IsAnnouncement,
                Title = x.Title,
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                Type = _repoStatus.FindById(x.Status).Description ?? "N/A"

            }).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> TabErickGetAll(string lang)
        {
            int index = 1;
            var data = (from x in await _repo.FindAll(x => x.Status == Suggession.Constants.Status.Dissatisfied).ToListAsync()
                        join y in _repoIdeaHis.FindAll() on x.Id equals y.IdeaID into last
                        let l = last.LastOrDefault()
                select new IdeaDto
            {
                Id = x.Id,
                SendID = x.SendID,
                ReceiveID = x.ReceiveID,
                CreatedTime = x.CreatedTime,
                CreatedBy = x.CreatedBy,
                Name = _repoAc.FindById(x.CreatedBy).FullName,
                Issue = x.Issue,
                Comment = l.Comment,
                Suggession = x.Suggession,
                Title = x.Title,
                Isshow = x.Isshow,
                IsAnnouncement = x.IsAnnouncement,
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                Type = _repoStatus.FindById(x.Status).Description ?? "N/A"

            }).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> TabCloseGetAll(string lang)
        {
            int index = 1;
            var data = (from x in await _repo.FindAll(x =>
            x.Status == Suggession.Constants.Status.Satisfied ||
            x.Status == Suggession.Constants.Status.Close).ToListAsync()
            join y in _repoIdeaHis.FindAll() on x.Id equals y.IdeaID into last
            let l = last.LastOrDefault()
            select new IdeaDto
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
                Comment = l .Comment,
                IsShowApproveTab = x.IsShowApproveTab,
                Isshow = x.Isshow,
                IsReject = _repoTimeLine.FindAll(y => y.IdeaID == x.Id).ToList().Count > 0 ? true : false,
                IsAnnouncement = x.IsAnnouncement,
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                Type = _repoStatus.FindById(x.Status).Description ?? "N/A"

            }).Where(x => x.IsShowApproveTab == false).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> TabApproveGetAll(string lang)
        {
            int index = 1;
            var data = (from x in await _repo.FindAll(x =>
            x.Status == Suggession.Constants.Status.Satisfied ||
            x.Status == Suggession.Constants.Status.Close).ToListAsync()
                        join y in _repoIdeaHis.FindAll() on x.Id equals y.IdeaID into last
                        let l = last.LastOrDefault()
                        select new IdeaDto
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
                            Comment = l.Comment,
                            IsShowApproveTab = x.IsShowApproveTab,
                            Isshow = x.Isshow,
                            IsAnnouncement = x.IsAnnouncement,
                            StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                            Type = _repoStatus.FindById(x.Status).Description ?? "N/A"

                        }).Where(x => x.IsShowApproveTab && !x.IsAnnouncement).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> TabAnnouncementGetAll(string lang)
        {
            int index = 1;
            var data = (from x in await _repo.FindAll(x =>
            x.Status == Suggession.Constants.Status.Satisfied ||
            x.Status == Suggession.Constants.Status.Close).ToListAsync()
                        join y in _repoPlanIdea.FindAll() on x.Id equals y.IdeaID  into last
                        let l = last.LastOrDefault()
                        select new IdeaDto
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
                            Comment = l == null ? "" : l.Plan,
                            IsShowApproveTab = x.IsShowApproveTab,
                            Isshow = x.Isshow,
                            IsAnnouncement = x.IsAnnouncement,
                            StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                            Type = _repoStatus.FindById(x.Status).Description ?? "N/A"

                        }).Where(x => x.IsAnnouncement && x.IsShowApproveTab).OrderByDescending(x => x.CreatedTime).ToList();
            data.ForEach(item =>
            {
                item.Index = index;
                index++;
            });
            return data;
        }

        public async Task<object> GetIdeaHisById(int id, string lang)
        {
            var index = 1;
            var data = await _repoIdeaHis.FindAll(x => x.IdeaID == id).Select(x => new IdeaDto { 
                Id = x.Id,
                IdeaId = x.IdeaID,
                Comment = lang == Constants.SystemLanguage.EN ? x.Comment != null ? x.Comment : "" : x.CommentZh != null ? x.CommentZh : "",
                Name = _repoAc.FindById(x.InsertBy).FullName ?? "",
                CreatedTime = x.CreatedTime,
                Issue = _repo.FindById(id).Issue,
                Suggession = _repo.FindById(id).Suggession,
                Sequence = index.ToString(),
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                Description = x.Status == Suggession.Constants.Status.ApproveStatus ? "0" : _repoStatus.FindById(x.Status) != null ? _repoStatus.FindById(x.Status).Description : null,
            }).ToListAsync();

            data.ForEach(item =>
            {
                item.Sequence = index.ToString();
                index++;
            });
            return data;
        }

        public async Task<object> GetIdeaHisByIdWithoutFactoryHead(int id, string lang)
        {
            var index = 1;
            var data = await _repoIdeaHis.FindAll(x => x.IdeaID == id).Select(x => new IdeaDto
            {
                Id = x.Id,
                IdeaId = x.IdeaID,
                Comment = lang == Constants.SystemLanguage.EN ? x.Comment != null ? x.Comment : "" : x.CommentZh != null ? x.CommentZh : "",
                Name = _repoAc.FindById(x.InsertBy).FullName ?? "",
                CreatedTime = x.CreatedTime,
                Issue = _repo.FindById(id).Issue,
                Suggession = _repo.FindById(id).Suggession,
                Sequence = index.ToString(),
                Status = x.Status,
                StatusName = lang == Constants.SystemLanguage.EN ? _repoStatus.FindById(x.Status).NameEn ?? "N/A" : _repoStatus.FindById(x.Status).NameZh ?? "N/A",
                Description = x.Status == Suggession.Constants.Status.ApproveStatus ? "0" : _repoStatus.FindById(x.Status) != null ? _repoStatus.FindById(x.Status).Description : null,
            }).Where(x => x.Status != Suggession.Constants.Status.ApproveStatus).ToListAsync();

            data.ForEach(item =>
            {
                item.Sequence = index.ToString();
                index++;
            });
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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

            catch (Exception)
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
            ideaHist.CommentZh = entity.Comment;
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
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> ErickApproval(IdeaDto entity)
        {
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.ApproveStatus;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            ideaHist.CommentZh = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _repoIdeaHis.SaveAll();

            var idea = _repo.FindById(entity.IdeaId);
            idea.IsAnnouncement = true;
            _repo.Update(idea);
            
            try
            {
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> ErickReject(IdeaDto entity)
        {
            //add moi vao bang ideaHistory
            var ideaHist = new IdeaHistory();
            ideaHist.IdeaID = entity.IdeaId;
            ideaHist.InsertBy = entity.CreatedBy;
            ideaHist.Status = Suggession.Constants.Status.ApproveStatus;
            ideaHist.Isshow = true;
            ideaHist.Comment = entity.Comment;
            ideaHist.CommentZh = entity.Comment;
            _repoIdeaHis.Add(ideaHist);
            await _repoIdeaHis.SaveAll();

            //add vao bang timeline
            var time_line = new TimeLine();
            time_line.Comment = entity.Comment;
            time_line.IdeaID = entity.IdeaId;
            time_line.CreatedBy = entity.CreatedBy;
            time_line.StatusName = Suggession.Constants.Status.Rejection;

            _repoTimeLine.Add(time_line);

            //update lai status bang idea
            var idea = _repo.FindById(entity.IdeaId);
            idea.IsShowApproveTab = false;

            try
            {
                await _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
