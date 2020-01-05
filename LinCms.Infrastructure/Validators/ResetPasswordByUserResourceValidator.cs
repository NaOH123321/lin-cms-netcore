using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Infrastructure.Validators
{
    public class ResetPasswordByUserResourceValidator : AbstractValidator<ResetPasswordByUserResource>, IFluentValidator
    {
        public ResetPasswordByUserResourceValidator()
        {
            RuleFor(x=>x.OldPassword)
                .NotEmpty()
                .WithName("原密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage);

            RuleFor(x => x).SetValidator(new PasswordValidator());
        }
    }
}
