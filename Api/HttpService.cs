using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;

namespace Api
{
    class HttpService
    {
        private HttpClient httpClient;
        private string baseAddress;

        public HttpService()
        {
            SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            baseAddress = Environment.GetEnvironmentVariable("BaseAddress", EnvironmentVariableTarget.Process) ?? string.Empty;
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
        }

        public async Task<string> Post(string uri, object body)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException("Base address has not been set in the environment");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseAddress}{uri}")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            return await SendRequest(request);
        }

        public async Task<string> SendRequest(HttpRequestMessage httpRequest)
        {
            using var response = await httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
