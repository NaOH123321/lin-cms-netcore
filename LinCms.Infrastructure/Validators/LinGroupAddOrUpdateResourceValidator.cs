using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources.LinGroups;

namespace LinCms.Infrastructure.Validators
{
    public class LinGroupAddOrUpdateResourceValidator<T> : AbstractValidator<T> where T : LinGroupAddOrUpdateResource
    {
        public LinGroupAddOrUpdateResourceValidator(LinContext linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("分组名称")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MaximumLength(60)
                .WithMessage(FluentValidatorMessage.MaxLengthMessage)
                .Must(x => !linContext.LinGroups.Any(g => g.Name == x))
                .WithMessage(FluentValidatorMessage.ExistedMessage);

            RuleFor(x => x.Info)
                .MaximumLength(255)
                .WithName("分组详情")
                .WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }
}
