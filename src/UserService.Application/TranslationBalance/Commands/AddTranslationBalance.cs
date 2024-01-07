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
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Application.TranslationBalance.Commands
{
    public class AddTranslationBalanceCommand : UpdateTranslationBalanceCommand
    {

    }

    public class AddTranslationBalanceHandler : IRequestHandler<AddTranslationBalanceCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AddTranslationBalanceHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> Handle(AddTranslationBalanceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Fail<UserDTO>(new EntityNotFoundError(request.UserId));
            }

            try
            {

            
            user.TranslationBalance += request.Amount;
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
