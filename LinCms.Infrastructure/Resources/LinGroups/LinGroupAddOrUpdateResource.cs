using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinGroups
{
    public class LinGroupAddOrUpdateResource
    {
        public string Name { get; set; } = null!;
        public string? Info { get; set; }
    }
}
