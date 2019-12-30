using System.ComponentModel.DataAnnotations;

namespace LinCms.Core
{
    public enum ResultCode
    {
        //通用错误类
        [Display(Name = "服务器错误")] 
        InternalServerErrorErrorCode = 99999,

        [Display(Name = "参数错误")] 
        BadRequestErrorCode = 10000,

        [Display(Name = "认证失败，请检查请求头或者重新登陆")]
        UnauthorizedErrorCode = 10010,

        [Display(Name = "权限不够，请联系超级管理员获得权限")] 
        ForbiddenErrorCode = 10011,

        [Display(Name = "无效Token")]
        UnauthorizedNotValidTokenErrorCode = 10021,

        [Display(Name = "Token已过期")]
        UnauthorizedTokenTimeoutErrorCode = 10022,

        [Display(Name = "控制器或方法不存在")]
        NotFoundErrorCode = 10030,

        [Display(Name = "请求的方法不被允许")]
        MethodNotAllowedErrorCode = 10031,

        [Display(Name = "不支持的MediaType")]
        UnsupportedMediaTypeErrorCode = 10040,

        [Display(Name = "不支持的Acceptable")]
        NotAcceptableErrorCode = 10050,


        [Display(Name = "资源不存在")]
        ResourceNotFoundErrorCode = 11000,

        [Display(Name = "权限验证不通过")]
        ResourceUnauthorizedErrorCode = 12000,

        //用户错误类
        [Display(Name = "用户不存在")]
        UserNotFoundErrorCode = 20000,

        [Display(Name = "密码错误，请输入正确密码")]
        UserPasswordErrorCode = 20010,

        [Display(Name = "您目前处于未激活状态，请联系超级管理员")]
        UserInactiveErrorCode = 21000,
    }
}
