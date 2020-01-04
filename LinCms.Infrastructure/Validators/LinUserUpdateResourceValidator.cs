using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Infrastructure.Validators
{
    public class LinUserUpdateResourceValidator : AbstractValidator<LinUserUpdateResource>
    {
        public LinUserUpdateResourceValidator(LinContext linContext, ICurrentUser currentUser)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithName("电子邮箱")
                .WithMessage(FluentValidatorMessage.RequiredMessage)
                .Must(x => string.IsNullOrWhiteSpace(x) ||
                           !linContext.LinUsers.Any(b => b.Email == x && b.Email != currentUser.Email))
                .WithMessage(FluentValidatorMessage.ExistedMessage);

            RuleFor(x => x.Nickname)
                .Length(2, 10)
                .WithName("昵称")
                .WithMessage(FluentValidatorMessage.MinLengthAndMaxLengthMessage);

            RuleFor(x => x.Avatar)
                .MaximumLength(255)
                .WithName("头像")
                .WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }
}
