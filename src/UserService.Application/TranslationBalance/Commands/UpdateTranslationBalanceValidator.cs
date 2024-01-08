using FluentValidation;

namespace UserService.Application.TranslationBalance.Commands
{
    public class UpdateTranslationBalanceValidator : AbstractValidator<UpdateTranslationBalanceCommand>
    {
        public UpdateTranslationBalanceValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Id field can't be empty.");

            RuleFor(v => v.Amount)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Amount field can't be empty.");
        }
    }
}