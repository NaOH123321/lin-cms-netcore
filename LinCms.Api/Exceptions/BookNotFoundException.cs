using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Exceptions
{
    public class BookNotFoundException : CommonException
    {
        public override int Code { get; } = StatusCodes.Status404NotFound;
        public override object Msg { get; set; } = "没有找到相关图书";
        public override ResultCode ErrorCode { get; set; } = ResultCode.NotFoundErrorCode;
    }
}
