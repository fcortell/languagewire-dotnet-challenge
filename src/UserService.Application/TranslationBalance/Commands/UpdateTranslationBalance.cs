using FluentResults;
using MediatR;
using UserService.Application.Users.Queries;

namespace UserService.Application.TranslationBalance.Commands
{
    public class UpdateTranslationBalanceCommand : IRequest<Result<UserDTO>>
    {
        // Amount to Add/Spend/Substract
        public int Amount { get; init; }

        public long UserId { get; init; }
    }
}