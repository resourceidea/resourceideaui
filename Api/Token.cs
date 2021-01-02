using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;

namespace Api
{
    public static class Token
    {
        [FunctionName("Token")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "token/{refresh?}")] HttpRequest req,
            string refresh,
            ILogger log)
        {
            log.LogInformation("Authentication token request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            try
            {
                string tokenResponse = string.Empty;
                if (string.IsNullOrEmpty(refresh))
                {
                    tokenResponse = await GetToken(data);
                }
                else
                {
                    tokenResponse = await RefreshToken(data);
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(tokenResponse,
                                                Encoding.UTF8,
                                                "application/json")
                };
            }
            catch (HttpRequestException error)
            {
                log.LogError(error.Message);
                throw;
            }

        }

        private static async Task<string> GetToken(dynamic data)
        {
            string username = data?.username;
            string password = data?.password;
            var refreshTokenResponse = await HandlePostRequest("/api/token/", new { username, password });
            return refreshTokenResponse;
        }

        private static async Task<string> RefreshToken(dynamic data)
        {
            string refresh = data?.refresh;
            var tokenResponse = await HandlePostRequest("/api/token/refresh/", new { refresh });
            return tokenResponse;
        }

        private static async Task<string> HandlePostRequest(string uri, object body)
        {
            var _httpService = new HttpService();
            var responseMessage = await _httpService.Post(uri, body);
            return responseMessage;
        }
    }
}
