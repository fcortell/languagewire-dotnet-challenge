using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace UserService.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.Email)
                .MaximumLength(100)
                .WithMessage("Email must not exceed 100 characters.");

            RuleFor(v => v.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email field can't be empty.");

            RuleFor(v => v.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format invalid.");

            RuleFor(v => v.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email field can't be empty.");
        }
    }
}
