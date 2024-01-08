
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Domain.Tiers;
using UserService.Domain.Tiers.Entities;
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
        private readonly ITierRepository _tierRepository;

        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, ITierRepository tierRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _tierRepository = tierRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? result = await _userRepository.GetByIdAsync(request.UserId);
            if (result is null)
            {
                return Result.Fail<UserDTO>(new EntityNotFoundError(request.UserId));
            }
            Tier tier = await _tierRepository.GetTierByRangeAsync(result.TranslationBalance);

            UserDTO dto = _mapper.Map<UserDTO>(result);
            dto.Tier = tier.Name;
            return dto;
        }
    }
}
