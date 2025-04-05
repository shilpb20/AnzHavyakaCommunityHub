using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using Microsoft.Extensions.Options;

namespace CommunityHub.UI.Services
{
    public class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IHttpRequestSender _requestSender;
        protected readonly AppSettings _appSettings;

        public BaseService(HttpClient httpClient, 
            IHttpRequestSender requestSender,
            IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
            _httpClient.BaseAddress = new Uri(_appSettings.ClientUrl.TrimEnd('/'));
            _requestSender = requestSender;
        }

        public async Task<ApiResponse<T>> GetRequestAsync<T>(string url)
        {
            var formattedUrl = EnsureBaseUrl(url);
            return await _requestSender.SendGetRequestAsync<T>(_httpClient, formattedUrl);
        }

        public async Task<ApiResponse<T>> GetRequestAsync<T>(string url, int id)
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            formattedUrl = EnsureBaseUrl(formattedUrl);
            return await _requestSender.SendGetRequestAsync<T>(_httpClient, formattedUrl);
        }

        public async Task<ApiResponse<V>> AddRequestAsync<T, V>(string url, T? data)
        {
            var formattedUrl = EnsureBaseUrl(url);
            return await _requestSender.SendPostRequestAsync<T, V>(_httpClient, formattedUrl, data);
        }

        public async Task<ApiResponse<V>> AddRequestAsync<T, V>(string url, int id, T? data)
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            formattedUrl = EnsureBaseUrl(formattedUrl);
            return await _requestSender.SendPostRequestAsync<T, V>(_httpClient, formattedUrl, data);
        }

        public async Task<ApiResponse<V>> UpdateRequestAsync<T, V>(string url, int id, T? data)
        {
            var formattedUrl = EnsureBaseUrl(url);
            return await _requestSender.SendUpdateRequestAsync<T, V>(_httpClient, formattedUrl, id, data);
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }

        private string EnsureBaseUrl(string url)
        {
            var baseUrl = _httpClient?.BaseAddress?.ToString();
            if (!url.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
            {
                return baseUrl.TrimEnd('/') + (url.StartsWith("/") ? url : "/" + url);
            }

            return url;
        }
    }
}
