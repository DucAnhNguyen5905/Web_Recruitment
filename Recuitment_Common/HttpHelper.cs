using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Recuitment_Common
{
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();


        public static async Task<HttpResponseMessage> SendHttpRequestAsync(HttpMethod method, string url, string accessToken, object? body = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
         new AuthenticationHeaderValue("Bearer", accessToken);

            using var request = new HttpRequestMessage(method, url);

            if (body != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");
            }

            return await _httpClient.SendAsync(request);
        }

        public static Task<HttpResponseMessage> SendGetAsync(HttpMethod method, string url, string accessToken, object? body = null)
        {
            return SendHttpRequestAsync(method, url, accessToken, body);
        }

        public static Task<HttpResponseMessage> SendPostAsync(HttpMethod method, string url, string accessToken, object? body = null)
        {
            return SendHttpRequestAsync(method, url, accessToken, body);
        }

        public static Task<HttpResponseMessage> SendPutAsync(HttpMethod method, string url, string accessToken, object? body = null)
        {
            return SendHttpRequestAsync(method, url, accessToken, body);
        }

        public static Task<HttpResponseMessage> SendDeleteAsync(HttpMethod method, string url, string accessToken, object? body = null)
        {
            return SendHttpRequestAsync(method, url, accessToken, body);
        }

    }
}
