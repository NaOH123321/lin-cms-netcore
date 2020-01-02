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
                .Length(2, 10)
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Continue)
                .Length(6, 22)
                .WithName("密码")
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage)
                .Matches("^[A-Za-z0-9_*&$#@]")
                .WithMessage("required|{PropertyName}需要包含字符、数字和 _");

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
