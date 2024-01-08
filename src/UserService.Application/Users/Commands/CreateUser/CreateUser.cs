using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users.Events;

namespace UserService.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<long>>
    {
        public string? Name { get; init; }

        public string? Email { get; init; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<long>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator? _mediator;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IMediator mediator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

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
                _userRepository.Insert(entity);

                await _userRepository.CommitChangesAsync(cancellationToken);

                if (_mediator is not null)
                {
                    await _mediator.Publish(new UserCreatedEvent(entity), cancellationToken);
                }

                return entity.Id;
            }
            catch (Exception ex)
            {
                Error error = new Error(ex.Message);
                return Result.Fail<long>(error);
            }
        }
    }
}