using AutoMapper;
using Suggession.DTO;
using Suggession.DTO.auth;
using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Helpers.AutoMapper
{
    public class EFToDtoMappingProfile : Profile
    {
        public EFToDtoMappingProfile()
        {

            CreateMap<Account, AccountDto>()
                .ForMember(d => d.AccountGroupText, o => o.MapFrom(s => s.AccountGroupAccount.Count > 0 ? String.Join(",", s.AccountGroupAccount.Select(x=>x.AccountGroup.Name)) : ""))
                .ForMember(d => d.AccountGroupIds, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Id) ))
                ;
            CreateMap<AccountGroup, AccountGroupDto>();
            CreateMap<Tab, TabDto>();

            CreateMap<Account, UserForDetailDto>()
                .ForMember(d => d.AccountGroupText, o => o.MapFrom(s => s.AccountGroupAccount.Count > 0 ? String.Join(",", s.AccountGroupAccount.Select(x => x.AccountGroup.Name)) : ""))
                .ForMember(d => d.AccountGroupPositions, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Position)))
                ;
            CreateMap<Comment, CommentDto>();
            CreateMap<AccountGroupAccount, AccountGroupAccountDto>();

            CreateMap<Idea, IdeaDto>();

        }
    }
}
