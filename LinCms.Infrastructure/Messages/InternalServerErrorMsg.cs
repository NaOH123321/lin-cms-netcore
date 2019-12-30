using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class InternalServerErrorMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status500InternalServerError;
        public override ResultCode ErrorCode { get; set; } = ResultCode.InternalServerErrorErrorCode;
    }
}
