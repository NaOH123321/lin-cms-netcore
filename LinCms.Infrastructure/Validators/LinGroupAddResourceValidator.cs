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
    public class LinGroupAddResourceValidator : LinGroupAddOrUpdateResourceValidator<LinGroupAddResource>
    {
        public LinGroupAddResourceValidator(LinContext linContext) : base(linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Auths)
                .NotEmpty()
                .WithName("权限字段数组")
                .WithMessage(FluentValidatorMessage.EmptyMessage);
        }
    }
}
