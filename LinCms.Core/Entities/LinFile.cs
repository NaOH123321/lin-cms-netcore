using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Entities
{
    public class LinFile : Entity
    {
        public string Path { get; set; } = null!;
        public short? Type { get; set; }
        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public int? Size { get; set; }
        public string? Md5 { get; set; }
    }
}
