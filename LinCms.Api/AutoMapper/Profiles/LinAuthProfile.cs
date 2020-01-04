using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Resources.LinAuths;

namespace LinCms.Api.AutoMapper.Profiles
{
    public class LinAuthProfile : Profile
    {
        public LinAuthProfile()
        {
            CreateMap<LinAuth, LinAuthResource>();

            CreateMap<IEnumerable<LinAuth>, IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>>>()
                .ConvertUsing<AuthsDisplayConvert>();
        }
    }
}
