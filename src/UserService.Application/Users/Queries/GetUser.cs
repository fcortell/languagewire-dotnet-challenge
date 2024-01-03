
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Users.Queries
{
    public record GetUserByIdQuery : IRequest<Result<UserDTO>>
    {
        public long UserId { get; init; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? result = await _userRepository.GetByIdAsync(request.UserId);
            UserDTO dto = _mapper.Map<UserDTO>(result);
            return dto;
        }
    }
}
