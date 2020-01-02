using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinUserProfile : Profile
    {
        public LinUserProfile()
        {
            CreateMap<LinUser, LinUserResource>();

            CreateMap<LinUserAddResource, LinUser>();
        }
    }
}
