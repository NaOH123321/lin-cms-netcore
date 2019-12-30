using LinCms.Core;
using Microsoft.AspNetCore.Http;

namespace LinCms.Infrastructure.Messages
{
    public class UnsupportedMediaTypeMsg : Message
    {
        public sealed override int Code { get; } = StatusCodes.Status415UnsupportedMediaType;
        public override ResultCode ErrorCode { get; set; } = ResultCode.UnsupportedMediaTypeErrorCode;
    }
}
