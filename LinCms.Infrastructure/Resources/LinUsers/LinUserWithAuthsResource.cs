using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Infrastructure.Resources.LinAuths;

namespace LinCms.Infrastructure.Resources.LinUsers
{
    public class LinUserWithAuthsResource : LinUserResource
    {
        public LinUserWithAuthsResource()
        {
            LinAuths = new List<Dictionary<string, IEnumerable<LinAuthResource>>>();
        }

        public IEnumerable<Dictionary<string, IEnumerable<LinAuthResource>>> LinAuths { get; set; }
    }
}
