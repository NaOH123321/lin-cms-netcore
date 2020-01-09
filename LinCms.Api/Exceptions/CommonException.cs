using System;
using System.Text.Json;
using LinCms.Core;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Extensions;

namespace LinCms.Api.Exceptions
{
    public abstract class CommonException : Exception, IMessage
    {
        private object? _message;

        public abstract int Code { get; }
        public virtual object Msg
        {
            get => _message ?? ErrorCode.GetDisplayName();
            set => _message = value;
        }
        public abstract ResultCode ErrorCode { get; set; }

        public virtual object ResponseMessage =>
            new
            {
                Code,
                ErrorCode,
                Msg
            };
    }
}
