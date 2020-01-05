using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Messages;

namespace LinCms.Infrastructure.Validators
{
    public class PasswordValidator : AbstractValidator<IPassword>
    {
        public PasswordValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("新密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage);

            RuleFor(x => x.Password)
                .Length(6, 22)
                .WithName("新密码")
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage)
                .Matches("^[A-Za-z0-9_*&$#@]")
                .WithMessage("required|{PropertyName}需要包含字符、数字和 _");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithName("确认新密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .Must((t, x) => x == t.Password)
                .WithMessage("required|两次输入的密码不一致，请输入相同的密码");
        }
    }
}
