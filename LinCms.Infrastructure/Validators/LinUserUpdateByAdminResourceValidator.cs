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
    public class LinUserUpdateByAdminResourceValidator : AbstractValidator<LinUserUpdateByAdminResource>
    {
        public LinUserUpdateByAdminResourceValidator(LinContext linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithName("电子邮箱")
                .WithMessage(FluentValidatorMessage.RequiredMessage);

            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithName("分组id")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .Must(x => linContext.LinGroups.Any(b => b.Id == x))
                .WithMessage(FluentValidatorMessage.NotExistedMessage);
        }
    }
}
