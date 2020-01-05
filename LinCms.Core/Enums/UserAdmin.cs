using System.ComponentModel.DataAnnotations;

namespace LinCms.Core.Enums
{
    public enum UserAdmin
    {
        [Display(Name = "普通用户")]
        Common = 1,

        [Display(Name = "超级管理员")]
        Admin = 2,
    }
}
