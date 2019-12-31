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
    public class LinUserLoginResourceValidator : AbstractValidator<LinUserLoginResource>
    {
        public LinUserLoginResourceValidator(LinContext linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithName("用户名")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .Must(x => linContext.LinUsers.Any(b => b.Username == x))
                .WithMessage(FluentValidatorMessage.NotExistedMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密码")
                .WithMessage(FluentValidatorMessage.EmptyMessage);
        }
    }
}
