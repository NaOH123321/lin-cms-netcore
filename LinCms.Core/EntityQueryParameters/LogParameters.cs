using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.EntityQueryParameters
{
    public class LogParameters : QueryParameters
    {
        public string? Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
