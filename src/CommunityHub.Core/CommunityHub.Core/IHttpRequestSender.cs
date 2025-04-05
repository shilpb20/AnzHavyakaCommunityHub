using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;

public interface IHttpRequestSender
{
    Task<ApiResponse<T>> SendGetRequestAsync<T>(HttpClient client, string url);
    Task<ApiResponse<V>> SendPostRequestAsync<T, V>(HttpClient client, string url, T? data);
    Task<ApiResponse<V>> SendUpdateRequestAsync<T, V>(HttpClient client, string url, int id, T? data);
    Task<ApiResponse<T>> SendUpdateRequestAsync<T>(HttpClient client, string url, int id, T? data);
}