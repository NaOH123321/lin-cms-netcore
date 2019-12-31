using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using LinCms.Infrastructure.Database;
using LinCms.Infrastructure.Messages;
using LinCms.Infrastructure.Resources;
using LinCms.Infrastructure.Resources.Books;

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
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MaximumLength(50)
                .WithMessage(FluentValidatorMessage.MaxLengthMessage)
                .Must(x => !linContext.Books.Any(b => b.Title == x))
                .WithMessage(FluentValidatorMessage.ExistedMessage);
            

            RuleFor(x => x.Author)
                .NotEmpty()
                .WithName("图书作者")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MaximumLength(30)
                .WithMessage(FluentValidatorMessage.MaxLengthMessage);

            RuleFor(x => x.Summary)
                .NotEmpty()
                .WithName("图书综述")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MaximumLength(1000)
                .WithMessage(FluentValidatorMessage.MaxLengthMessage);

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithName("图书插图")
                .WithMessage(FluentValidatorMessage.EmptyMessage)
                .MaximumLength(50)
                .WithMessage(FluentValidatorMessage.MaxLengthMessage);
        }
    }
}
