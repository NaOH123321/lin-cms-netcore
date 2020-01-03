using System;
using System.Collections.Generic;
using System.Text;
using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class CreatedMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status201Created;
        public override ResultCode ErrorCode { get; set; } = ResultCode.CreatedCode;
    }
}
