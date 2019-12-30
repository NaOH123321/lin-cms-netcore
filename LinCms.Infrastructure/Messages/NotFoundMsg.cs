using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class NotFoundMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status404NotFound;
        public override ResultCode ErrorCode { get; set; } = ResultCode.NotFoundErrorCode;
    }
}
