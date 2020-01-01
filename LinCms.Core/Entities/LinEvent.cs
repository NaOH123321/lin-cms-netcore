using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core.Entities
{
    public class LinEvent : IEntityInt
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string? MessageEvents { get; set; }

        public LinGroup LinGroup { get; set; } = null!;
    }
}
