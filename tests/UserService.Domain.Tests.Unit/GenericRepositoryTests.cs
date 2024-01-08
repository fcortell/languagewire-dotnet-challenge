using System.Linq.Expressions;
using System.Threading;
using AutoFixture;
using Moq;
using UserService.Domain.Users;
using UserService.Domain.Users.Entities;

namespace UserService.Domain.Test.Unit
{
    public class GenericRepositoryTests
    {
        private readonly Mock<IUserRepository> _userMockRepository;
        private Fixture _fixture;

        public GenericRepositoryTests()
        {
            _fixture = new Fixture();
            _userMockRepository = new Mock<IUserRepository>();
        }

        [Fact()]
        public void CommitChangesAsyncTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.Update(It.IsAny<User>())).Callback(() => { return; });

            // Act

            _userMockRepository.Object.Update(testUser);
            _userMockRepository.Object.CommitChangesAsync(CancellationToken.None);

            // Assert
            _userMockRepository.Verify(d => d.CommitChangesAsync(CancellationToken.None), Times.Once());
        }

        [Fact()]
        public void DeleteTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.Delete(It.IsAny<User>())).Callback(() => { return; });

            // Act

            _userMockRepository.Object.Delete(testUser);

            // Assert
            _userMockRepository.Verify(d => d.Delete(It.IsAny<User>()), Times.Once());
        }

        [Fact()]
        public void FindTest()
        {
            // Arrange
            var userList = _fixture.CreateMany<User>(10).ToList();
            _userMockRepository.Setup(p => p.Find(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userList);

            // Act
            var result = _userMockRepository.Object.Find(x => x.Email == userList.First().Email).Result;

            // Assert
            Assert.True(userList.First() == result.First());
        }

        [Fact()]
        public void GetAllAsyncTest()
        {
            // Arrange
            var userList = _fixture.CreateMany<User>(10).ToList();
            _userMockRepository.Setup(p => p.GetAllAsync()).ReturnsAsync(userList);

            // Act
            var result = _userMockRepository.Object.GetAllAsync().Result;

            // Assert
            Assert.True(userList.Count() == result.Count());
        }

        [Fact()]
        public void GetByIdAsyncTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(testUser);

            // Act
            var result = _userMockRepository.Object.GetByIdAsync(testUser.Id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.True(testUser.Id == result.Id);
        }

        [Fact()]
        public void InsertTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.Insert(It.IsAny<User>())).Callback(() => { return; });

            // Act
            _userMockRepository.Object.Insert(testUser);

            //Assert
            _userMockRepository.Verify(d => d.Insert(It.IsAny<User>()), Times.Once());
        }

        [Fact()]
        public void UpdateTest()
        {
            // Arrange
            var testUser = _fixture.Create<User>();
            _userMockRepository.Setup(p => p.Update(It.IsAny<User>())).Callback(() => { return; });

            // Act

            _userMockRepository.Object.Update(testUser);

            // Assert
            _userMockRepository.Verify(d => d.Update(It.IsAny<User>()), Times.Once());
        }
    }
}