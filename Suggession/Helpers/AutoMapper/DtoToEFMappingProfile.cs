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
    public class DtoToEFMappingProfile : Profile
    {
        public DtoToEFMappingProfile()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<TabDto, Tab>();

            CreateMap<AccountTypeDto, AccountType>()
                .ForMember(d => d.Accounts, o => o.Ignore());
            CreateMap<AccountGroupDto, AccountGroup>();

     
            CreateMap<MailingDto, Mailing>();


            CreateMap<UserForDetailDto, Account>();

            CreateMap<CommentDto, Comment>();
            CreateMap<ContributionDto, Contribution>();
            CreateMap<OCDto, OC>();
            CreateMap<AccountGroupAccountDto, AccountGroupAccount>();

            CreateMap<OCAccountDto, OCAccount>();
            
            CreateMap<IdeaDto, Idea>();
        }
    }
}
