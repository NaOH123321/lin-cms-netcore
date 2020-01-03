using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Api.Services;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinUserProfile : Profile
    {
        public LinUserProfile()
        {
            //CreateMap<LinUser, LinUserResource>()
            //    .ForMember();

            CreateMap<ICurrentUser, LinUserResource>();
            CreateMap<ICurrentUser, LinUserWithAuthsResource>()
                .ForMember(dest => dest.LinAuths, opt => opt.MapFrom<AuthsDisplayResolver>());

            CreateMap<LinUserAddResource, LinUser>();
        }
    }
}
