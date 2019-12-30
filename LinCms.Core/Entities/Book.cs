using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core.Entities
{
    public class Book : Entity
    {
        public string Title { get; set; } = null!;
        public string? Author { get; set; }
        public string? Summary { get; set; }
        public string? Image { get; set; }
    }
}
