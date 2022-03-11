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
            CreateMap<AccountGroupDto, AccountGroup>();
            CreateMap<UserForDetailDto, Account>();
            CreateMap<CommentDto, Comment>();
            CreateMap<AccountGroupAccountDto, AccountGroupAccount>();
            CreateMap<IdeaDto, Idea>();
            CreateMap<PlanIdeaDto, PlanIdea>();
            CreateMap<StatusDto, Status>();
        }
    }
}
