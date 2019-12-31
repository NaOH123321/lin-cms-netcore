using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinUsers
{
    public class LinUserResource
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }
        public short Admin { get; set; }
        public short Active { get; set; }
        public string? Email { get; set; }
        public int? GroupId { get; set; }
    }
}
