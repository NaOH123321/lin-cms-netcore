using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class NotAcceptableMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status406NotAcceptable;
        public override ResultCode ErrorCode { get; set; } = ResultCode.NotAcceptableErrorCode;
    }
}
