using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinCms.Core
{
    public enum UserAdmin
    {
        [Display(Name = "普通用户")]
        Common = 1,

        [Display(Name = "超级管理员")]
        Admin = 2,
    }
}
