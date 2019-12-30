using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class ForbiddenMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status403Forbidden;
        public override ResultCode ErrorCode { get; set; } = ResultCode.ForbiddenErrorCode;
    }
}
