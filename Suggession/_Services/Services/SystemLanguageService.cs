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
using Microsoft.EntityFrameworkCore;
using NetUtility;

namespace Suggession._Services.Services
{
    
    public class SystemLanguageService :  ISystemLanguageService
    {
        private readonly ISystemLanguageRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public SystemLanguageService(
            ISystemLanguageRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<bool> Add(Models.SystemLanguage model)
        {
            _repo.Add(model);
            return await _repo.SaveAll();
        }

        public async Task<bool> Delete(int id)
        {
            var item = _repo.FindById(id);
            _repo.Remove(item);
            return await _repo.SaveAll();
        }

        public async Task<object> GetLanguages(string lang)
        {
            var data = (await _repo.FindAll().AsNoTracking().Select(x => new {
                x.SLKey,
                x.SLType,
                Name = lang == "zh" ? x.SLTW : x.SLEN,
            }).Select(t => new
            {
                t.SLKey,
                t.Name,
                t.SLType
            }).ToListAsync()).DistinctBy(x => x.SLKey);
            var languages = data.ToDictionary(t => t.SLKey, t => t.Name);
            return languages;
        }

        public Models.SystemLanguage GetById(int id) => _repo.FindById(id);

        public async Task<PagedList<Models.SystemLanguage>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repo.FindAll().OrderByDescending(x => x.ID);
            return await PagedList<Models.SystemLanguage>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<PagedList<Models.SystemLanguage>> Search(PaginationParams param, object text)
        {
            var lists = _repo.FindAll()
            .OrderByDescending(x => x.ID);
            return await PagedList<Models.SystemLanguage>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public async Task<bool> Update(Models.SystemLanguage model)
        {
            _repo.Update(model);
            return await _repo.SaveAll();
        }

        public Task<bool> UpdateLanguage()
        {
            //var data = new List<Models.SystemLanguage>();
            //string json = System.IO.File.ReadAllText(@"wwwroot/Language/" + "en.json");
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //foreach (var items in jsonObj)
            //{
            //    var data2 = new Models.SystemLanguage();
            //    data2.SLKey = items.Key;
            //    data2.SLEN = items.Value;
            //    data.Add(data2);
            //}
            //_repo.AddRange(data);
            //return _repo.SaveAll();

            var dataExist = _repo.FindAll().ToList();
            var data = new List<Models.SystemLanguage>();
            string json = System.IO.File.ReadAllText(@"wwwroot/Language/" + "zh.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (var item in dataExist)
            {
                foreach (var items in jsonObj)
                {
                    if (item.SLKey == items.Key)
                    {
                        item.SLTW = items.Value;
                    }
                }
            }
            _repo.UpdateRange(dataExist);
            return _repo.SaveAll();


            //string root = @"C:\users";
            //string root2 = @"C:\Users";

            //bool result = root.Equals(root2);
            //Console.WriteLine($"Ordinal comparison: <{root}> and <{root2}> are {(result ? "equal." : "not equal.")}");

            //result = root.Equals(root2, StringComparison.Ordinal);
            //Console.WriteLine($"Ordinal comparison: <{root}> and <{root2}> are {(result ? "equal." : "not equal.")}");

            //Console.WriteLine($"Using == says that <{root}> and <{root2}> are {(root == root2 ? "equal" : "not equal")}");
        }

        public async Task<object> GetAllAsync()
        {
            return await _repo.FindAll().OrderByDescending(X => X.ID).ToListAsync();
            throw new NotImplementedException();
        }
    }

   
}
