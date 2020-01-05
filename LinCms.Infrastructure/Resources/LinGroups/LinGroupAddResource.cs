using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinGroups
{
    public class LinGroupAddResource : LinGroupAddOrUpdateResource
    {
        public LinGroupAddResource()
        {
            Auths = new List<string>();
        }

        public IEnumerable<string> Auths { get; set; }
    }
}
