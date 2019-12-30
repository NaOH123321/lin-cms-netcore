using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core.Entities
{
    public abstract class Entity : IEntityInt
    {
        public int Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
