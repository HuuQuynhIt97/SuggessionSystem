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

namespace Suggession._Services.Interface
{
    public interface IIdeaService
    {
        //Task<object> GetKPIByOcID(int ocID);
        //Task<object> GetPolicyByOcID(int ocID);
        Task<object> TabProposalGetAll(string lang);
        Task<object> TabProcessingGetAll(string lang);
        Task<object> TabErickGetAll(string lang);
        Task<object> TabCloseGetAll(string lang);
        Task<object> TabApproveGetAll(string lang);
        Task<object> TabAnnouncementGetAll(string lang);
        Task<object> GetIdeaHisById(int id, string lang);
        Task<object> GetIdeaHisByIdWithoutFactoryHead(int id, string lang);
        Task<bool> UploadFile(IdeaDto entity);
        Task<bool> EditSuggession(IdeaDto entity);
        Task<bool> EditSubmitSuggession(IdeaDto entity);
        Task<bool> Delete(int id);
        Task<bool> Accept(IdeaDto entity);
        Task<bool> Reject(IdeaDto entity);
        Task<bool> Update(IdeaDto entity);
        Task<bool> UpdateAnnouncement(int id);
        Task<bool> Close(IdeaDto entity);
        Task<bool> Complete(IdeaDto entity);
        Task<bool> Terminate(IdeaDto entity);
        Task<bool> ErickApproval(IdeaDto entity);
        Task<bool> ErickReject(IdeaDto entity);
        Task<bool> Satisfied(IdeaDto entity);
        Task<bool> Dissatisfied(IdeaDto entity);
    }
   
}
