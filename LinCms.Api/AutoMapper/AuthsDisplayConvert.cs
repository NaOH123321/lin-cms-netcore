using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Resources.LinAuths;

namespace LinCms.Api.AutoMapper
{
    public class AuthsDisplayConvert : ITypeConverter<IEnumerable<LinAuth>, IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>>>
    {
        private readonly IMapper _mapper;

        public AuthsDisplayConvert(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>> Convert(IEnumerable<LinAuth> source, IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>> destination, ResolutionContext context)
        {
            var listAuths = source.ToLookup(a => a.Module);

            var auths = new List<Dictionary<string, IEnumerable<LinAuthResource>>>();

            foreach (var listAuth in listAuths)
            {
                var dic = new Dictionary<string, IEnumerable<LinAuthResource>>();
                var linAuthResources = _mapper.Map<IEnumerable<LinAuth>, IEnumerable<LinAuthResource>>(listAuth);
                dic.Add(listAuth.Key, linAuthResources);
                auths.Add(dic);
            }

            return auths;
        }
    }
}
