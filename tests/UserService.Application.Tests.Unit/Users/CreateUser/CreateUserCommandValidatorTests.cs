using AutoMapper;
using FluentAssertions;
using Moq;
using UserService.Application.Users.Commands.CreateUser;
using UserService.Domain.Users;

namespace UserService.Application.Tests.Unit.Users.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailFormatIsInvalid()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = "test@com2"
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailIsEmpty()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = ""
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailIsNotProvided()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test"
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailIsNull()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = null
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmailIsTooLong()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = "0j1z3kt8ujgvtcepdw0zqm0bvyn9weh9jjqtzkf19440u1jid6f3542zi2zp8ezjag9f4nxc7aki0u6pq7wf9p884a9wnrvvrfztt2cc6@test.com"
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenNameIsNotProvided()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Email = "test@test.com"
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccessResult_WhenAllParametersAreOk()
        {
            // Arrange
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Name = "Test",
                Email = "test@test.com"
            };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}