using CommunityHub.Core.Factory;

namespace CommunityHub.Core.Helpers.Tests
{
    public class HttpSendRequestHelperTests
    {
        private readonly HttpClient _httpClient;
        private IResponseFactory _responseFactory;
        //private readonly AspNetHttpRequestSender _httpRequestSender;

        public HttpSendRequestHelperTests()
        {
            _responseFactory = new ResponseFactory();
            //_httpRequestSender = new HttpRequestSender(_responseFactory);

            _httpClient = new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };
        }

        [Fact]
        public async Task SendGetRequestAsync_ShouldReturnData_OnSuccessfulRequest()
        {
            var url = "/todos/1";

            var result = await _httpRequestSender.SendGetRequestAsync<object>(_httpClient, url);

            Assert.NotNull(result);
            Assert.Contains("userId", result.ToString());
        }

        [Fact]
        public async Task SendGetRequestAsync_ShouldThrowException_OnHttpRequestError()
        {
            var url = "/invalidendpoint";

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _httpRequestSender.SendGetRequestAsync<object>(_httpClient, url));

            Assert.Contains("An error occurred while sending the GET request.", exception.Message);
        }

        //[Fact]
        //public async Task SendPostRequestAsync_ShouldReturnData_OnSuccessfulPost()
        //{
        //    var url = "/todos";
        //    var newTodo = new
        //    {
        //        userId = 1,
        //        title = "Test Todo",
        //        completed = false
        //    };

        //    var result = await HttpSendRequestHelper.SendPostRequestAsync<object, object>(_httpClient, url, newTodo);

        //    Assert.NotNull(result);
        //    Assert.Equal("Test Todo", ((dynamic)result).title);
        //}

        [Fact]
        public async Task SendPostRequestAsync_ShouldThrowException_OnHttpRequestError()
        {
            var url = "/invalidendpoint";
            var newTodo = new { userId = 1, title = "Test Todo", completed = false };

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _httpRequestSender.SendPostRequestAsync<object, object>(_httpClient, url, newTodo));

            Assert.Contains("An error occurred while sending the POST request.", exception.Message);
        }

        //[Fact]
        //public async Task SendUpdateRequestAsync_ShouldReturnData_OnSuccessfulPut()
        //{
        //    var url = "/todos";
        //    var updateTodo = new
        //    {
        //        userId = 1,
        //        id = 1,
        //        title = "Updated Todo",
        //        completed = true
        //    };
        //    var todoId = 1;

        //    var result = await HttpSendRequestHelper.SendUpdateRequestAsync<object, object>(_httpClient, url, todoId, updateTodo);

        //    Assert.NotNull(result);
        //    Assert.Equal("Updated Todo", ((dynamic)result).title);
        //}

        [Fact]
        public async Task SendUpdateRequestAsync_ShouldThrowException_OnHttpRequestError()
        {
            var url = "/invalidendpoint";
            var updateTodo = new { userId = 1, id = 1, title = "Updated Todo", completed = true };
            var todoId = 999;

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _httpRequestSender.SendUpdateRequestAsync<object, object>(_httpClient, url, todoId, updateTodo));

            Assert.Contains("An error occurred while sending the PUT request.", exception.Message);
        }
    }
}
