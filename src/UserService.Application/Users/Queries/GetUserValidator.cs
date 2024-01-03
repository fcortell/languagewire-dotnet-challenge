using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
