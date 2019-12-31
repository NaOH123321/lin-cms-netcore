using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinCms.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAttribute : Attribute
    {
        public string Msg { get; set; }

        public LogAttribute(string msg)
        {
            Msg = msg;
        }
    }
}
