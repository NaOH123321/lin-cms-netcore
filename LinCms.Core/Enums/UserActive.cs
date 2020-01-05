using System.ComponentModel.DataAnnotations;

namespace LinCms.Core.Enums
{
    public enum UserActive
    {
        [Display(Name = "激活状态")]
        Active = 1,

        [Display(Name = "非激活状态")]
        NotActive = 2,
    }
}
