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

namespace Suggession._Services.Services
{
    
    public class TabService :  ITabService
    {
        private readonly ITabRepository _repo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public TabService(
            ITabRepository repo, 
            IMapper mapper, 
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> GetAll()
        {
            var data = _repo.FindAll().Select(x => new TabDto
            { 
                Id = x.Id,
                Name = x.Name,
                Statues = false
            }).ToList();
            foreach (var item in data)
            {
                if (item.Name == "Proposal")
                {
                    item.Statues = true;
                }
            }
            return data;
        }
    }

   
}
