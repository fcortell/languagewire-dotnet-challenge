using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using UserService.Application.Users.Queries;

namespace UserService.Application.TranslationBalance.Commands
{
    public class UpdateTranslationBalanceCommand : IRequest<Result<UserDTO>>
    {
        public long UserId { get; init; }

        // Amount to Add/Spend/Substract
        public int Amount { get; init; }
    }
}
