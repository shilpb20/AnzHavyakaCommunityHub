
using Newtonsoft.Json;
using System.Text;
using System.Web;
using JsonException = Newtonsoft.Json.JsonException;

namespace CommunityHub.Core.Helpers
{
    public class HttpHelper
    {
        public static string BuildUri(string baseUrl, string path, Dictionary<string, string> queryParams)
        {
            var uriBuilder = new UriBuilder(new Uri(new Uri(baseUrl), path));
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }


        public static HttpRequestMessage GetHttpGetRequestById(string url, int id)
        {
            var formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            return GetHttpGetRequest(formattedUrl);
        }

        public static HttpRequestMessage GetHttpGetRequest(string url)
        {
            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        public static HttpRequestMessage GetHttpPutRequest<T>(string url, int id, T? data)
        {
            var formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, formattedUrl);

            var json = data != null ? JsonConvert.SerializeObject(data) : null;
            if (json != null)
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
            }
            else
            {
                requestMessage.Content = null;
            }

            return requestMessage;
        }

        public static HttpRequestMessage GetHttpPutRequest(string url)
        {
            return new HttpRequestMessage(HttpMethod.Put, url);
        }

        public static HttpRequestMessage GetHttpPostRequest<T>(string url, int id, T? data)
        {
            string formattedUrl = ApiRouteHelper.FormatRoute(url, id);
            return GetHttpPostRequest<T>(formattedUrl, data);
        }

        public static HttpRequestMessage GetHttpPostRequest<T>(string url, T? data)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            var json = data != null ? JsonConvert.SerializeObject(data) : null;
            if (json != null)
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
            }
            else
            {
                requestMessage.Content = null;
            }

            return requestMessage;
        }

        public static async Task<T?> GetHttpResponseObject<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseString))
            {
                return default;
            }

            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (JsonException)
            {
                return default;
            }
        }
    }
}
