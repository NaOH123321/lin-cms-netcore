using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core.Entities
{
    public class LinGroup : IEntityInt
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }
    }
}
