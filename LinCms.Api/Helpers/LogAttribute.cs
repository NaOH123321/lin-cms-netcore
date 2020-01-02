using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinCms.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAttribute : Attribute
    {
        public string Template { get; set; }

        public LogAttribute(string template)
        {
            Template = template;
        }
    }
}
