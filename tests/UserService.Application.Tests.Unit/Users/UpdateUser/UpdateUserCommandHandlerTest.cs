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
using UserService.Application.Users.Commands.CreateUser;
using UserService.Application.Users.Commands.UpdateUser;
using UserService.Application.Users.Queries;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Application.Tests.Unit.Users.UpdateUser
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private Fixture _fixture;

        public UpdateUserCommandHandlerTest()
        {
            _fixture = new Fixture();
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
            var command = new UpdateUserCommand
            {
                Id = 1,
                Name = "Test",
                Email = "test@test.com"
            };

            var users = _fixture.CreateMany<User>(2).ToList();


            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((long i) => users.First(c => c.Id == users.First().Id));
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync(new Domain.Users.Entities.User() { Email = command.Email });

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new UniqueConstraintViolationError("User", "Email"));
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEntityNotFound()
        {
            // Arrange
            var command = new UpdateUserCommand
            {
                Id = 1,
                Name = "Test",
                Email = "test@test.com"
            };
            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _mapper);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id)).Callback(() => { return; });

            // Act
            Result<UserDTO> result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainEquivalentOf(new EntityNotFoundError(command.Id));
        }
    }
}
