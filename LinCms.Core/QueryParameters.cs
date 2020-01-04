using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using LinCms.Core.Interfaces;

namespace LinCms.Core
{
    public abstract class QueryParameters : INotifyPropertyChanged
    {
        //每页默认的数量
        private const int DefaultCount = 15;
        //每页最大默认数量
        private const int DefaultMaxCount = 100;

        private int _start;
        /// <summary>
        /// 跳过几条数据
        /// </summary>
        public int Start
        {
            get => _start;
            set => _start = value >= 0 ? value : 0;
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

        private int _count = DefaultCount;
        /// <summary>
        /// 每页的数量
        /// </summary>
        public virtual int Count
        {
            get => _count;
            set => SetField(ref _count, value);
        }

        private string? _orderBy;
        public string? OrderBy
        {
            get => _orderBy;
            set => _orderBy = value ?? nameof(IEntity<object>.Id);
        }

        private int _maxCount = DefaultMaxCount;
        /// <summary>
        /// 每页最大的数量
        /// </summary>
        protected internal virtual int MaxCount
        {
            get => _maxCount;
            set => SetField(ref _maxCount, value);
        }

        public string? Fields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            if (propertyName == nameof(Count) || propertyName == nameof(MaxCount))
            {
                SetCount();
            }
            return true;
        }

        private void SetCount()
        {
            if (_maxCount <= 0)
            {
                _maxCount = DefaultMaxCount;
            }
            if (_count <= 0)
            {
                _count = DefaultCount;
            }
            _count = _count > _maxCount ? _maxCount : _count;
        }
    }
}
