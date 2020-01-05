using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Api.Services;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Core.Enums;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.LinAuths;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinUserProfile : Profile
    {
        public LinUserProfile()
        {
            CreateMap<LinUser, LinUserResource>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Active == (short) UserActive.Active))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.Admin == (short) UserAdmin.Admin))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.LinGroup.Name));

            CreateMap<ICurrentUser, LinUserResource>();
            CreateMap<ICurrentUser, LinUserWithAuthsResource>()
                .ForMember(dest => dest.Auths, opt => opt.MapFrom(src => src.Auths));

            CreateMap<LinUserAddResource, LinUser>();
            CreateMap<LinUserUpdateResource, LinUser>();
            CreateMap<LinUserUpdateByAdminResource, LinUser>();
        }
    }
}
