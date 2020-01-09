using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Resources.LinFiles
{
    public class LinFileResource
    {
        public string Key { get; set; } = null!;
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
