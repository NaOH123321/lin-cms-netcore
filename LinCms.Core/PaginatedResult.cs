using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Core
{
    public class PaginatedResult<T>
    {
        public PaginatedResult()
        {
            Items = new List<T>();
        }

        public int Count { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
