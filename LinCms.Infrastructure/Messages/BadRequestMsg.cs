using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class BadRequestMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status400BadRequest;
        public override ResultCode ErrorCode { get; set; } = ResultCode.BadRequestErrorCode;
    }
}
