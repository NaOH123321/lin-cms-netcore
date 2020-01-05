using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Infrastructure.Resources.LinUsers
{
    public class ResetPasswordByUserResource : IPassword
    {
        public string OldPassword { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
