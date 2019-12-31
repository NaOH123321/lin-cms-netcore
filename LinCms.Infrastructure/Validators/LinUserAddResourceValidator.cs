using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Infrastructure.Validators
{
    public class LinUserAddResourceValidator : AbstractValidator<LinUserAddResource>
    {
        public LinUserAddResourceValidator(LinContext linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithName("用户名")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MinimumLength(2)
                .MaximumLength(10)
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MinimumLength(6)
                .MaximumLength(22)
                .Matches("^[A-Za-z0-9_*&$#@]")
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage + "，包含字符、数字和 _");

            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithName("分组id")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .Must(x => linContext.LinGroups.Any(b => b.Id == x))
                .WithMessage(FluentValidatorMessage.NotExistedMessage);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithName("电子邮箱")
                .WithMessage(FluentValidatorMessage.RequiredMessage);
        }
    }
}
