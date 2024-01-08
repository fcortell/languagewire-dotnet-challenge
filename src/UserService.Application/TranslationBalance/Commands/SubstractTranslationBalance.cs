using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using UserService.Application.Common.Errors;
using UserService.Application.Users.Queries;
using UserService.Domain.Tiers;
using UserService.Domain.Tiers.Entities;
using UserService.Domain.Users;

namespace UserService.Application.TranslationBalance.Commands
{
    public class SubstractTranslationBalanceCommand : UpdateTranslationBalanceCommand
    {

    }

    public class SubstractTranslationBalanceHandler : IRequestHandler<SubstractTranslationBalanceCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITierRepository _tierRepository;
        private readonly IMapper _mapper;

        public SubstractTranslationBalanceHandler(IUserRepository userRepository, ITierRepository tierRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _tierRepository = tierRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(SubstractTranslationBalanceCommand request, CancellationToken cancellationToken)
    {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
        {
                return Result.Fail<UserDTO>(new EntityNotFoundError(request.UserId));
            }
            try { 

                Tier tier = await _tierRepository.GetTierByRangeAsync(user.TranslationBalance);
            user.TranslationBalance -= request.Amount;

            _userRepository.Update(user);
            await _userRepository.CommitChangesAsync(cancellationToken);
            UserDTO dto = _mapper.Map<UserDTO>(user);
            dto.Tier = tier.Name;
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
