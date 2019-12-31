using System;
using System.Collections.Generic;
using System.Text;

namespace LinCms.Infrastructure.Messages
{
    public class FluentValidatorMessage
    {
        /// <summary>
        /// 必填
        /// 要求PropertyName
        /// </summary>
        public static readonly string EmptyMessage = @"required|{PropertyName}是必填的";

        /// <summary>
        /// 最大长度
        /// 要求PropertyName
        /// 要求MaxLength
        /// </summary>
        public static readonly string MaxLengthMessage = @"maxlength|{PropertyName}的最大长度是{MaxLength}";

        /// <summary>
        /// 最小长度
        /// 要求PropertyName
        /// 要求MinLength
        /// </summary>
        public static readonly string MinLengthMessage = @"minlength|{PropertyName}的最小长度是{MinLength}";

        /// <summary>
        /// 范围
        /// 要求PropertyName
        /// 要求MinLength
        /// 要求MaxLength
        /// </summary>
        public static readonly string MinLengthAndMaxLengthMessage = @"min-maxlength|{PropertyName}长度必须在{MinLength}~{MaxLength}之间";

        /// <summary>
        /// 已存在
        /// 要求PropertyName
        /// </summary>
        public static readonly string ExistedMessage = @"existed|{PropertyName}已存在";

        /// <summary>
        /// 不存在
        /// 要求PropertyName
        /// </summary>
        public static readonly string NotExistedMessage = @"notExisted|{PropertyName}不存在，请输入正确的{PropertyName}";

        /// <summary>
        /// 不符合规范
        /// 要求PropertyName
        /// </summary>
        public static readonly string RequiredMessage = @"required|{PropertyName}不符合规范，请输入正确的{PropertyName}";
    }
}
