using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using LinCms.Core.Interfaces;
using LinCms.Infrastructure.Resources.LinUsers;

namespace LinCms.Infrastructure.Validators
{
    public class ResetPasswordByAdminResourceValidator : AbstractValidator<ResetPasswordByAdminResource>, IFluentValidator
    {
        public ResetPasswordByAdminResourceValidator()
        {
            RuleFor(x => x).SetValidator(new PasswordValidator());
        }
    }
}
