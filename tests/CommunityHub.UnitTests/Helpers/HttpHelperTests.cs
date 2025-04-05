using CommunityHub.Core.Helpers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class HttpHelperTests
{

    [Fact]
    public void GetHttpGetRequestById_CreatesCorrectRequest()
    {
        // Arrange
        var url = "https://api.example.com/{id:int}";
        var id = 123;

        // Act
        var request = HttpHelper.GetHttpGetRequestById(url, id);

        // Assert
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("https://api.example.com/123", request.RequestUri.ToString());
    }

    [Fact]
    public void GetHttpPutRequest_CreatesCorrectRequest_WithSerializedData()
    {
        // Arrange
        var url = "https://api.example.com/{id:int}";
        var id = 123;
        var data = new { Name = "Test" };

        // Act
        var request = HttpHelper.GetHttpPutRequest(url, id, data);

        // Assert
        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal("https://api.example.com/123", request.RequestUri.ToString());
        Assert.Equal("{\"Name\":\"Test\"}", request.Content.ReadAsStringAsync().Result);
    }

    [Fact]
    public void BuildUri_CreatesCorrectUri_WithQueryParams()
    {
        // Arrange
        var baseUrl = "https://api.example.com:8080/{id:int}";
        var path = "/endpoint";
        var queryParams = new Dictionary<string, string> { { "key", "value" } };

        // Act
        var result = HttpHelper.BuildUri(baseUrl, path, queryParams);

        // Assert
        Assert.Equal("https://api.example.com:8080/endpoint?key=value", result);
    }
}
