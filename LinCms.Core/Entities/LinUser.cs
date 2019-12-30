using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Entities
{
    public class LinUser : Entity
    {
        public string Username { get; set; } = null!;
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }
        public short Admin { get; set; }
        public short Active { get; set; }
        public string? Email { get; set; }
        public int? GroupId { get; set; }
        public string? Password { get; set; }
    }
}
