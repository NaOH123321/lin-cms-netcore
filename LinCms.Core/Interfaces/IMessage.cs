using LinCms.Core;

namespace LinCms.Core.Interfaces
{
    public interface IMessage
    {
        int Code { get; }
        object Msg { get; set; }
        ResultCode ErrorCode { get; set; }
    }
}
