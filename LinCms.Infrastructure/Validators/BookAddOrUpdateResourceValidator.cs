using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Resources;

namespace LinCms.Infrastructure.Validators
{
    public class BookAddOrUpdateResourceValidator : AbstractValidator<BookAddOrUpdateResource>
    {
        public BookAddOrUpdateResourceValidator(LinContext linContext)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithName("图书名")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(50)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}")
                .Must(x => !linContext.Books.Any(b => b.Title == x))
                .WithMessage("existed|{PropertyName}已存在");
            

            RuleFor(x => x.Author)
                .NotEmpty()
                .WithName("图书作者")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(30)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithName("图书综述")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(1000)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithName("图书插图")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(50)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");
        }
    }
}
