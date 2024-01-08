using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Domain.Tiers.Entities;
using UserService.Domain.Tiers;
using UserService.Domain.Users.Entities;
using UserService.Domain.Users;

namespace UserService.Application.Users.Queries
{
    public record GetUsersQuery : IRequest<Result<List<UserDTO>>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly ITierRepository _tierRepository;
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository, ITierRepository tierRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _tierRepository = tierRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<UserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in users)
            {
                UserDTO dto = _mapper.Map<UserDTO>(user);

                Tier tier = await _tierRepository.GetTierByRangeAsync(user.TranslationBalance);
                dto.Tier = tier.Name;
                result.Add(dto);
            }

            return result;
        }
    }
}
