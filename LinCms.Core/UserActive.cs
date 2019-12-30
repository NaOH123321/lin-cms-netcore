using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinCms.Core
{
    public enum UserActive
    {
        [Display(Name = "激活状态")]
        Active = 1,

        [Display(Name = "非激活状态")]
        NotActive = 2,
    }
}
