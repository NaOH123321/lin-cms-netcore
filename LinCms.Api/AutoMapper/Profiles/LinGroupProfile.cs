using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Resources.LinGroups;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinGroupProfile : Profile
    {
        public LinGroupProfile()
        {
            CreateMap<LinGroup, LinGroupResource>();
            CreateMap<LinGroup, LinGroupWithAuthsResource>()
                .ForMember(dest => dest.Auths, opt => opt.MapFrom(src => src.LinAuths));

            CreateMap<LinGroupAddResource, LinGroup>();
            CreateMap<LinGroupUpdateResource, LinGroup>();
        }
    }
}
