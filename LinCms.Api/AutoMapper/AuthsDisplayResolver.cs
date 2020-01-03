using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinCms.Core.Entities;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Resources.LinAuths;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Api.AutoMapper
{
    public class AuthsDisplayResolver : IValueResolver<ICurrentUser, LinUserWithAuthsResource, IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>>>
    {
        public IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>> Resolve(ICurrentUser source,
            LinUserWithAuthsResource destination, IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>> destMember,
            ResolutionContext context)
        {
            var listAuths = source.Auths.Select(a=>a.GroupId);

            var auths = new List<Dictionary<string, IEnumerable<LinAuthResource>>>();

            foreach (var listAuth in listAuths)
            {
                var LinAuthResources = new List<LinAuthResource>();

                
           
            }

            return auths;
        }
    }
}
