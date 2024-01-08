using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using UserService.Application.TranslationBalance.Commands;
using UserService.Application.Users.Queries;
using UserService.Domain.Tiers;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Tests.Unit.TranslationBalance
{
public class UpdateTranslationBalanceCommandValidatorTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITierRepository> _tierRepositoryMock;

        public UpdateTranslationBalanceCommandValidatorTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tierRepositoryMock = new Mock<ITierRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Add_Handler_Should_ReturnFailureResult_WhenUserIdIsNotProvided()
        {
            // Arrange
            var validator = new UpdateTranslationBalanceValidator();
            var command = new AddTranslationBalanceCommand
            {
                Amount = 10
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Add_Handler_Should_ReturnFailureResult_WhenAmountIsNotProvided()
        {
            // Arrange
            var validator = new UpdateTranslationBalanceValidator();
            var command = new AddTranslationBalanceCommand
            {
                UserId = 10
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
