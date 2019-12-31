using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Exceptions
{
    public class BadRequestException : CommonException
    {
        public override int Code { get; } = StatusCodes.Status400BadRequest;
        public override ResultCode ErrorCode { get; set; } = ResultCode.BadRequestErrorCode;
    }
}
