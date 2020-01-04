using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Api.Exceptions
{
    public class ForbiddenException : CommonException
    {
        public override int Code { get; } = StatusCodes.Status403Forbidden;
        public override ResultCode ErrorCode { get; set; } = ResultCode.ResourceForbiddenErrorCode;
    }
}
