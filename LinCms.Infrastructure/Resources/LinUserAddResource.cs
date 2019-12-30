using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources
{
    public class LinUserAddResource
    {
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public int GroupId { get; set; }
    }
}
