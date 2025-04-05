using CommunityHub.Core.Helpers;
using Newtonsoft.Json.Linq;

namespace CommunityHub.UnitTests.Helpers
{
    public class JsonHelperTests
    {
        [Theory]
        [InlineData(null, false)]
        public void IsEmptyJson_ShouldReturnFalse_ForNullObject(object input, bool expected)
        {
            // Act
            var result = JsonHelper.IsEmptyJson(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnTrue_ForEmptyAnonymousObject()
        {
            // Arrange
            var obj = new { };

            // Act
            var result = JsonHelper.IsEmptyJson(obj);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnTrue_ForEmptyDictionary()
        {
            // Arrange
            var dict = new Dictionary<string, string>();

            // Act
            var result = JsonHelper.IsEmptyJson(dict);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnFalse_ForNonEmptyObject()
        {
            // Arrange
            var obj = new { Name = "John" };

            // Act
            var result = JsonHelper.IsEmptyJson(obj);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnFalse_ForEmptyList()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = JsonHelper.IsEmptyJson(list);

            // Assert
            Assert.False(result); // Lists serialize as [] not {}, so false
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnFalse_ForNonEmptyList()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var result = JsonHelper.IsEmptyJson(list);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEmptyJson_ShouldReturnFalse_ForJsonElement()
        {
            // Arrange
            var jsonElement = JObject.Parse("{\"ValueKind\":1}");

            // Act
            var result = JsonHelper.IsEmptyJson(jsonElement);

            // Assert
            Assert.False(result);
        }
    }
}


