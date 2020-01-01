using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LinCms.Api.Helpers
{
    public class PermissionMetaRequirement : IAuthorizationRequirement
    {
        public string? Name { get; set; }

        public string Role { get; set; } = null!;
    }
}
