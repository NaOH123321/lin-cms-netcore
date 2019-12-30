using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core.Entities
{
    public class LinGroup : IEntityInt
    {
        public LinGroup()
        {
            LinAuths = new List<LinAuth>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }

        public IList<LinAuth> LinAuths { get; set; }
    }
}
