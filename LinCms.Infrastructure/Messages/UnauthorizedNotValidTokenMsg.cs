using LinCms.Core;

namespace LinCms.Infrastructure.Messages
{
    public class UnauthorizedNotValidTokenMsg : UnauthorizedMsg
    {
        public override ResultCode ErrorCode { get; set; } = ResultCode.UnauthorizedNotValidTokenErrorCode;
    }
}
