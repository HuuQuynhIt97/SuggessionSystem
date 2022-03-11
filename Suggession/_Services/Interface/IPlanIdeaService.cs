using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession._Services.Interface
{
    public interface IPlanIdeaService
    {
        Task<List<PlanIdeaDto>> GetPlanIdea(int id);
        Task<List<PlanIdeaDto>> GetPlanOkIdea(int id);
        Task<object> GetFactoryHeadComment(int id);
        Task<List<PlanStatus>> GetPlanStatus();

        Task<bool> Add(List<PlanIdea> model);
        Task<bool> CreateOrUpdate(List<PlanIdea> model);
        Task<bool> Update(List<PlanIdea> model);
        Task<bool> Delete(int id);
        Task<bool> SubmitPlanIdea(int id);
    }
    

   
}
