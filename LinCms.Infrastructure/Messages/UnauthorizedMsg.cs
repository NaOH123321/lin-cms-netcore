using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class UnauthorizedMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status401Unauthorized;
        public override ResultCode ErrorCode { get; set; } = ResultCode.UnauthorizedErrorCode;
    }
}
