using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users.Events;
using UserService.Domain.Users;
using UserService.Application.Users.Queries;

namespace UserService.Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result<UserDTO>>
    {
        public long Id { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMediator? _mediator;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IMediator mediator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate user exists
            User user = await _userRepository.GetByIdAsync(request.Id);
            if (user is null)
            {
                return Result.Fail<UserDTO>(new EntityNotFoundError(request.Id));
            }
            // Validate email does not exist
            if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            {
                return Result.Fail<UserDTO>(new UniqueConstraintViolationError(nameof(User), nameof(User.Email)));
            }

            try
            {
                user.Name = request.Name;
                user.Email = request.Email;

                _userRepository.Update(user);

                await _userRepository.CommitChangesAsync(cancellationToken);
                UserDTO dto = _mapper.Map<UserDTO>(user);

                return dto;
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message);
                return Result.Fail<UserDTO>(error);

            }
        }
    }
}
