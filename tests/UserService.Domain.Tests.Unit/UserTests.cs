using Xunit;

namespace UserService.Domain.Test.Unit
{
	public class UserTests
	{
		[Fact]
		public void CanBeCreated()
		{
			// Arrange, Act
			var result = "Robert Lewandosky";
			var user = new
			{
				Name = "Robert Lewandosky",
			};

			// Assert
			//result.Should().BeSuccess();
		}

		[Fact]
		public void CannotBeEmpty()
		{
			// Arrange, Act
			var result = "";

			// Assert
			//result.Should().BeFailure();
		}

		[Fact]
		public void CannotBeLargerThan100Chars()
		{
			// Arrange
			var name = string.Concat(Enumerable.Repeat("a", 101));

			// Act
			var result = name;

			// Assert
			//result.Should().BeFailure();
		}
	}
}