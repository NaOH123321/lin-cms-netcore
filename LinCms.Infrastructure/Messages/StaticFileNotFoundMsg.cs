using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core;

namespace LinCms.Infrastructure.Messages
{
    public class StaticFileNotFoundMsg: NotFoundMsg
    {
        public override ResultCode ErrorCode { get; set; } = ResultCode.StaticFileNotFoundErrorCode;
    }
}
