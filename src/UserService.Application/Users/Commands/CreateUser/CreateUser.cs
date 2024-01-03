using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using UserService.Application.Common.Interfaces;
using UserService.Application.Users.Queries;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users.Events;
using FluentResults;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using UserService.Application.Common.Errors;

namespace UserService.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<long>>
    {
        public string? Name { get; init; }

        public string? Email { get; init; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<long>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate email does not exist
            if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            {
                return Result.Fail<long>(new UniqueConstraintViolationError(nameof(User), nameof(User.Email)));
            }

            var entity = new User
            {
                Name = request.Name,
                Email = request.Email,
            };

            try
            {
                entity.AddDomainEvent(new UserCreatedEvent(entity));

                _userRepository.Insert(entity);

                await _userRepository.CommitChangesAsync(cancellationToken);
                return entity.Id;
            } catch (Exception ex)
            {
                Error error = new Error(ex.Message);
                return Result.Fail<long>(error);
         
            }
        }
    }
}
