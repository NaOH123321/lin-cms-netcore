using LinCms.Core;

namespace LinCms.Infrastructure.Messages
{
    public class UnauthorizedTokenTimeoutMsg : UnauthorizedMsg
    {
        public override ResultCode ErrorCode { get; set; } = ResultCode.UnauthorizedTokenTimeoutErrorCode;
    }
}
