﻿using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Infrastructure.Resources.LinUsers
{
    public class LinUserAddResource : IPassword
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string? Email { get; set; }
        public int GroupId { get; set; }
    }
}
