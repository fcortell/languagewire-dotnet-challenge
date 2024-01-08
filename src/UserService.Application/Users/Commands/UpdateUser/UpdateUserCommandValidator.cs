using FluentValidation;

namespace UserService.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            //RuleFor(m => m.Id).NotEmpty().When(m => string.IsNullOrEmpty(m.Email));
            //RuleFor(m => m.Email).NotEmpty().When(m => m.Id > 0);

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

            RuleFor(v => v.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Id field can't be empty.");

            RuleFor(v => v.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email field can't be empty.");
        }
    }
}