using CommunityHub.Core.Helpers;
using FluentAssertions;

namespace CommunityHub.UnitTests.Helpers
{
    public class ApiRouteHelperTests
    {
        [Theory]
        [InlineData("api/users/{id:int}", 1, "api/users/1")]
        [InlineData("api/account/register/{id:int}", 42, "api/account/register/42")]
        [InlineData("api/admin/approve/{id:int}", 1000, "api/admin/approve/1000")]
        [InlineData("api/admin/reject/{id:int}", 99, "api/admin/reject/99")]
        [InlineData("api/users/{id:int}/details", 567, "api/users/567/details")]
        public void FormatRoute_ShouldReplaceIdPlaceholder_WithGivenId(string route, int id, string expectedRoute)
        {
            var result = ApiRouteHelper.FormatRoute(route, id);
            result.Should().Be(expectedRoute);
        }

        [Fact]
        public void FormatRoute_ShouldReturnOriginalRoute_WhenNoIdPlaceholder()
        {
            string route = "api/users";
            int id = 123;
            var result = ApiRouteHelper.FormatRoute(route, id);
            result.Should().Be(route);
        }

        [Fact]
        public void FormatRoute_ShouldHandleEmptyRoute()
        {
            string route = "";
            int id = 123;
            var result = ApiRouteHelper.FormatRoute(route, id);
            result.Should().Be("");
        }
    }
}

