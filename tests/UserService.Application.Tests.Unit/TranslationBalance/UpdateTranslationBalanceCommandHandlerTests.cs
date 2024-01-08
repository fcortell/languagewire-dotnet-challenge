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
using UserService.Application.Common.Errors;
using UserService.Application.TranslationBalance.Commands;
using UserService.Application.Users.Commands.UpdateUser;
using UserService.Application.Users.Queries;
using UserService.Domain.Tiers;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Tests.Unit.TranslationBalance
{
public class UpdateTranslationBalanceCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITierRepository> _tierRepositoryMock;
        private Fixture _fixture;

        public UpdateTranslationBalanceCommandHandlerTests()
        {
            _fixture = new Fixture();
            _tierRepositoryMock = new Mock<ITierRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Add_Handler_Should_ReturnFailureResult_WhenEntityNotFound()
        {
            // Arrange
            var command = new AddTranslationBalanceCommand
            {
                UserId = 1,
                Amount = 100
            };
            var handler = new AddTranslationBalanceHandler(_userRepositoryMock.Object, _tierRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId)).Callback(() => { return; });

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new EntityNotFoundError(command.UserId));
        }

        [Fact]
        public async Task Spend_Handler_Should_ReturnFailureResult_WhenEntityNotFound()
        {
            // Arrange
            var command = new SpendTranslationBalanceCommand
            {
                UserId = 1,
                Amount = 100
            };
            var handler = new SpendTranslationBalanceHandler(_userRepositoryMock.Object, _tierRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId)).Callback(() => { return; });

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new EntityNotFoundError(command.UserId));
        }

        [Fact]
        public async Task Spend_Handler_Should_ReturnFailureResult_WhenBalanceInsufficient()
        {
            var users = _fixture.CreateMany<User>(2).ToList();

            // Arrange
            var command = new SpendTranslationBalanceCommand
            {
                UserId = 1,
                Amount = users.First().TranslationBalance + 100
            };
            var handler = new SpendTranslationBalanceHandler(_userRepositoryMock.Object, _tierRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((long i) => users.First(c => c.Id == users.First().Id));

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new InsufficientTranslationBalanceError(command.UserId));
        }

        [Fact]
        public async Task Substract_Handler_Should_ReturnFailureResult_WhenEntityNotFound()
        {
            // Arrange
            var command = new SubstractTranslationBalanceCommand
            {
                UserId = 1,
                Amount = 100
            };
            var handler = new SubstractTranslationBalanceHandler(_userRepositoryMock.Object, _tierRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId)).Callback(() => { return; });

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new EntityNotFoundError(command.UserId));
        }
    }
}
