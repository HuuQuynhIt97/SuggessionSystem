using AutoMapper;
using Suggession.Data;
using Suggession.DTO;
using Suggession.Models;
using Suggession._Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suggession.Helpers;

namespace Suggession._Services.Interface
{
    public interface ISystemLanguageService
    {
        Task<object> GetLanguages(string lang);
        Task<object> GetAllAsync();
        Task<bool> UpdateLanguage();
        Task<PagedList<Models.SystemLanguage>> Search(PaginationParams param, object text);
        SystemLanguage GetById(int id);
        Task<bool> Add(SystemLanguage model);
        Task<bool> Update(SystemLanguage model);
        Task<bool> Delete(int id);
        Task<PagedList<Models.SystemLanguage>> GetWithPaginations(PaginationParams param);
    }
    

   
}
