using AutoFixture;
using Moq;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Domain.Test.Unit
{
    public class IUserRepositoryTests
    {
        private readonly Mock<IUserRepository> _userMockRepository;
        private Fixture _fixture;

        public IUserRepositoryTests()
        {
            _fixture = new Fixture();
            _userMockRepository = new Mock<IUserRepository>();
        }

        [Fact()]
        public void GetByEmailAsyncTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(testUser);

            // Act
            var result = _userMockRepository.Object.GetByEmailAsync(testUser.Email).Result;

            // Assert
            Assert.NotNull(result);
            Assert.True(testUser.Email == result.Email);
        }
    }
}