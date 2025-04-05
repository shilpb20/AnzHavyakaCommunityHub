using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CommunityHub.UI.Services
{
    public class HttpRequestSender : IHttpRequestSender
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResponseFactory _responseFactory;
        private readonly ICookieReaderService _cookieService;
        private readonly ICookieStorage _cookieStorage;

        public HttpRequestSender(
            IHttpContextAccessor httpContextAccessor,
            IResponseFactory responseFactory,
            ICookieReaderService cookieService,
            ICookieStorage cookieStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _responseFactory = responseFactory;
            _cookieService = cookieService;

            _cookieStorage = cookieStorage;
        }

        public async Task<ApiResponse<T>> SendGetRequestAsync<T>(HttpClient client, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                return await SendAndProcessResponse<T>(client, request);
            }
            catch (HttpRequestException)
            {
                return _responseFactory.Failure<T>(ErrorCode.RequestError, "An error occurred while sending the GET request.");
            }
            catch
            {
                return _responseFactory.Failure<T>(ErrorCode.UnknownError, "An unexpected error occurred in sending the GET request.");
            }
        }

        public async Task<ApiResponse<V>> SendPostRequestAsync<T, V>(HttpClient client, string url, T? data)
        {
            try
            {
                HttpRequestMessage request = HttpHelper.GetHttpPostRequest<T>(url, data);
                return await SendAndProcessResponse<V>(client, request);
            }
            catch (HttpRequestException)
            {
                return _responseFactory.Failure<V>(ErrorCode.RequestError, "An error occurred while sending the POST request.");
            }
            catch
            {
                return _responseFactory.Failure<V>(ErrorCode.UnknownError, "An unexpected error occurred in sending the POST request.");
            }
        }

        public async Task<ApiResponse<T>> SendUpdateRequestAsync<T>(HttpClient client, string url, int id, T? data)
        {
            return await SendUpdateRequestAsync<T, T>(client, url, id, data);
        }

        public async Task<ApiResponse<V>> SendUpdateRequestAsync<T, V>(HttpClient client, string url, int id, T? data)
        {
            try
            {
                string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
                var uri = new Uri(client.BaseAddress, formattedUrl);
                HttpRequestMessage request = HttpHelper.GetHttpPutRequest<T>(uri.ToString(), id, data);
                return await SendAndProcessResponse<V>(client, request);
            }
            catch (HttpRequestException)
            {
                return _responseFactory.Failure<V>(ErrorCode.RequestError, "An error occurred while sending the PUT request.");
            }
            catch
            {
                return _responseFactory.Failure<V>(ErrorCode.UnknownError, "An unexpected error occurred in sending the PUT request.");
            }
        }

        private async Task<ApiResponse<T>> SendAndProcessResponse<T>(HttpClient client, HttpRequestMessage request)
        {
            request = SetAuthHeader(request);
            var httpResponse = await client.SendAsync(request);
            StoreCookies(httpResponse);

            return await ProcessResponse<T>(httpResponse);
        }

        private HttpRequestMessage SetAuthHeader(HttpRequestMessage request)
        {
            if (_cookieStorage.IsCookiePresent())
            {
                var authToken = _cookieStorage.GetCookie(CookiePart.AuthToken);
                if (!string.IsNullOrEmpty(authToken))
                {
                    request.Headers.Add("Authorization", "Bearer " + authToken.Trim());

                }
            }

            return request;
        }

        private void StoreCookies(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                var cookies = _cookieService.ReadCookiesFromResponse(httpResponse);
                foreach (var cookie in cookies)
                {
                    var cookieExpiration = _cookieService.GetCookieExpiry(cookie.Key);
                    _cookieStorage.SetCookie(cookie.Key, cookie.Value, cookieExpiration);
                }
            }
        }

        private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();

                string errorCode;
                string errorMessage;
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

                    errorCode = errorObj.ErrorCode ?? ErrorCode.HttpError;
                    errorMessage = errorObj.ErrorMessage ?? errorContent;
                }
                catch
                {
                    errorCode = ErrorCode.HttpError;
                    errorMessage = errorContent;
                }

                return _responseFactory.Failure<T>(errorCode, errorMessage);
            }

            try
            {
                var data = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(data))
                    return _responseFactory.Success<T>(default(T));

                var parsedData = JsonConvert.DeserializeObject<ApiResponse<T>>(data);
                return _responseFactory.Success<T>(parsedData.Data);
            }
            catch (Exception ex)
            {
                return _responseFactory.Failure<T>(ErrorCode.ResponseParseError, $"An error occurred while processing the response: {ex.Message}");
            }
        }
    }
}
