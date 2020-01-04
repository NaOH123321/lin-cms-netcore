using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinUsers
{
    public class LinUserAddResource
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public int GroupId { get; set; }
    }
}
