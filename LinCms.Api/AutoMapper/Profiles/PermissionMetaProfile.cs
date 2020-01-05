using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core;
using LinCms.Core.Entities;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class PermissionMetaProfile : Profile
    {
        public PermissionMetaProfile()
        {
            CreateMap<PermissionMeta, LinAuth>();
        }
    }
}
