using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class MethodNotAllowedMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status405MethodNotAllowed;
        public override ResultCode ErrorCode { get; set; } = ResultCode.MethodNotAllowedErrorCode;
    }
}
