using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Interfaces
{
    public interface IPassword
    {
        string Password { get; set; }
        string ConfirmPassword { get; set; }
    }
}
