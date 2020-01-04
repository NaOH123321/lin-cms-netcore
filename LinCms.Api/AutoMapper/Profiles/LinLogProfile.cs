using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Resources.LinLogs;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinLogProfile : Profile
    {
        public LinLogProfile()
        {
            CreateMap<LinLog, LinLogResource>();
        }
    }
}
