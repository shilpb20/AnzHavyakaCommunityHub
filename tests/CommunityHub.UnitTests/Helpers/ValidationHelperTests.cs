using CommunityHub.Core.Helpers;
using Xunit;

namespace CommunityHub.Core.Tests.Helpers
{
    public class ValidationHelperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateNullString_ShouldThrowArgumentException_ForNullOrWhitespaceValues(string invalidValue)
        {
            // Arrange
            string parameterName = "testParam";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ValidationHelper.ValidateNullOrEmptyString(invalidValue, parameterName)
            );

            Assert.Contains("testParam cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ValidateNullString_ShouldNotThrow_ForValidString()
        {
            // Arrange
            string validValue = "Valid String";
            string parameterName = "testParam";

            // Act & Assert
            ValidationHelper.ValidateNullOrEmptyString(validValue, parameterName);
        }
    }
}