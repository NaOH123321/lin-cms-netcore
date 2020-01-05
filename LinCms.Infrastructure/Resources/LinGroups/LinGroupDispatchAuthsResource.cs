using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinGroups
{
    public class LinGroupDispatchAuthsResource
    {
        public LinGroupDispatchAuthsResource()
        {
            Auths = new List<string>();
        }
        public int GroupId { get; set; }

        public IEnumerable<string> Auths { get; set; }
    }
}
