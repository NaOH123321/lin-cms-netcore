using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core;

namespace LinCms.Api.AutoMapper
{
    public class PermissionMetasDisplayConvert : ITypeConverter<IEnumerable<PermissionMeta>, Dictionary<string, Dictionary<string, IEnumerable<string>>>>
    {
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> Convert(IEnumerable<PermissionMeta> source, Dictionary<string, Dictionary<string, IEnumerable<string>>> destination, ResolutionContext context)
        {
            var permissionMetaLookups = source.ToLookup(a => a.Module);

            var result = new Dictionary<string, Dictionary<string, IEnumerable<string>>>();

            foreach (var permissionMetaLookup in permissionMetaLookups)
            {
                var auths = new Dictionary<string, IEnumerable<string>>();

                var authsLookups = permissionMetaLookup.ToLookup(p => p.Auth);

                foreach (var authsLookup in authsLookups)
                {
                    var authRouteNameList = authsLookup.Select(l => $"{l.RouteName}+{l.MethodName}");
                    auths.Add(authsLookup.Key, authRouteNameList);
                }

                result.Add(permissionMetaLookup.Key, auths);
            }

            return result;
        }
    }
}
