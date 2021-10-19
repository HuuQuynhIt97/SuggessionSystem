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
            CreateMap<AccountType, AccountTypeDto>();
            CreateMap<AccountGroup, AccountGroupDto>();
            CreateMap<Tab, TabDto>();
            CreateMap<Mailing, MailingDto>();
            CreateMap<AccountGroupPeriod, AccountGroupPeriodDto>();

            CreateMap<Account, UserForDetailDto>()
                .ForMember(d => d.AccountGroupText, o => o.MapFrom(s => s.AccountGroupAccount.Count > 0 ? String.Join(",", s.AccountGroupAccount.Select(x => x.AccountGroup.Name)) : ""))
                .ForMember(d => d.AccountGroupPositions, o => o.MapFrom(s => s.AccountGroupAccount.Select(x=>x.AccountGroup.Position)))
                ;
            CreateMap<Comment, CommentDto>();
            CreateMap<Contribution, ContributionDto>();
            CreateMap<OC, OCDto>();
            CreateMap<AccountGroupAccount, AccountGroupAccountDto>();

            CreateMap<OCAccount, OCAccountDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.Account.FullName));
            CreateMap<OCPolicy, OCPolicyDto>();
            CreateMap<Idea, IdeaDto>();

        }
    }
}
