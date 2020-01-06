using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core
{
    public class PermissionMeta
    {
        public string? Auth { get; set; }
        public string? Module { get; set; }
        public string Role { get; set; } = null!;
        public bool Mount { get; set; }
        public string? MethodName { get; set; } 
        public string RouteName { get; set; } = null!;
    }
}
