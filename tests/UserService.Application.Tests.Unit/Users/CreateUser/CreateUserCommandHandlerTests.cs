using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using UserService.Application.Common.Errors;
using UserService.Application.Users.Commands.CreateUser;
using UserService.Domain.Users;

namespace UserService.Application.Tests.Unit.Users.CreateUser
{
    public class CreateUserCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public CreateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailIsNotUnique()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = "test@test.com"
            };

            var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync(new Domain.Users.Entities.User() { Email = command.Email });

            // Act
            Result<long> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new UniqueConstraintViolationError("User", "Email"));
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenEmailIsUnique()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = "test@test.com"
            };

            var handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).Callback(() => { return; });

            // Act
            Result<long> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}