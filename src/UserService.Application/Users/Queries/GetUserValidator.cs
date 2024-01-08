using FluentValidation;

namespace UserService.Application.Users.Queries
{
    public sealed class GetUserValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty();
        }
    }
}