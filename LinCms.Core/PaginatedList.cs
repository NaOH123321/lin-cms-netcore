using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core
{
    public class PaginatedList<T> : List<T> where T : class
    {
        /// <summary>
        /// 数据真实的个数，相当于List.Count
        /// </summary>
        public int ActualCount => base.Count;

        private int _count;
        /// <summary>
        /// 每页的数量
        /// </summary>
        public new int Count
        {
            get => _count;
            set => _count = value >= 1 ? value : 1;
        }

        private int _page;
        /// <summary>
        /// 从第几页开始
        /// </summary>
        public int Page         
        {
            get => _page;
            set => _page = value >= 0 ? value : 0;
        }

        private int _total;
        /// <summary>
        /// 总个数
        /// </summary>
        public int Total
        {
            get => _total;
            set => _total = value >= 0 ? value : 0;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage => Total / Count + (Total % Count > 0 ? 1 : 0);

        public bool HasPrevious => Page > 0;
        public bool HasNext => Page < TotalPage - 1;

        public PaginatedList(int page, int count, int total, IEnumerable<T> data)
        {
            Page = page;
            Count = count;
            Total = total;
            AddRange(data);
        }
    }
}
