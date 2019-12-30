using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Resources;

namespace LinCms.Infrastructure.Validators
{
    public class LinUserLoginResourceValidator : AbstractValidator<LinUserLoginResource>
    {
        public LinUserLoginResourceValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithName("用户名")
                .WithMessage("required|{PropertyName}是必填的");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密码")
                .WithMessage("required|{PropertyName}是必填的");
        }
    }
}
