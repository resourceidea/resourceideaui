using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Api
{
    class HttpService
    {
        private HttpClient httpClient;
        private string baseAddress;
        private HttpServiceResponse serviceResponse;

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

        public async Task<HttpServiceResponse> Post(string uri, object body, string token = null)
        {
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException("Base address has not been set in the environment");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseAddress}{uri}")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            return await SendRequest(request, token);
        }

        public async Task<HttpServiceResponse> Get(string uri, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseAddress}{uri}");
            return await SendRequest(request, token);
        }

        public async Task<HttpServiceResponse> SendRequest(HttpRequestMessage request, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            using var response = await httpClient.SendAsync(request);
            serviceResponse.StatusCode = response.StatusCode;
            if (response.IsSuccessStatusCode)
            {
                serviceResponse.Content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                serviceResponse.Content = await response.Content?.ReadAsStringAsync();
            }

            return serviceResponse;
        }
    }
}
