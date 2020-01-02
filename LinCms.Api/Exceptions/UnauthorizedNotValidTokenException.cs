using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Core;

namespace LinCms.Api.Exceptions
{
    public class UnauthorizedNotValidTokenException : UnauthorizedException
    {
        public override ResultCode ErrorCode { get; set; } = ResultCode.UnauthorizedNotValidTokenErrorCode;
    }
}
