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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "token/{refresh?}")] HttpRequest req,
            string refresh,
            ILogger log)
        {
            log.LogInformation("INFO: Initiating authentication token request");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            HttpResponseMessage responseMessage;
            try
            {
                HttpServiceResponse tokenResponse;
                if (string.IsNullOrEmpty(refresh))
                {
                    tokenResponse = await GetToken(data);
                }
                else
                {
                    tokenResponse = await RefreshToken(data);
                }

                responseMessage = new HttpResponseMessage(tokenResponse.StatusCode);

                if (tokenResponse.StatusCode == HttpStatusCode.OK)
                {
                    responseMessage.Content = new StringContent(tokenResponse.Content,
                                                                Encoding.UTF8,
                                                                "application/json");

                    log.LogInformation($"INFO: Authentication token request successful");
                }
                else
                {
                    responseMessage.Content = new StringContent("Authentication token request failed",
                                                                Encoding.UTF8,
                                                                "application/json");

                    log.LogError($"ERROR: Authentication token request FAILED with response {tokenResponse.Content}");
                }
            }
            catch (HttpRequestException error)
            {
                log.LogError($"Authentication token request failed with error: {error.Message}");
                responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Authentication token request failed",
                                                Encoding.UTF8,
                                                "application/json")
                };
            }

            return responseMessage;

        }

        private static async Task<HttpServiceResponse> GetToken(dynamic data)
        {
            string username = data?.username;
            string password = data?.password;
            return await HandlePostRequest("/api/token/", new { username, password });
        }

        private static async Task<HttpServiceResponse> RefreshToken(dynamic data)
        {
            string refresh = data?.refresh;
            return await HandlePostRequest("/api/token/refresh/", new { refresh });
        }

        private static async Task<HttpServiceResponse> HandlePostRequest(string uri, object body)
        {
            var _httpService = new HttpService();
            return await _httpService.Post(uri, body);
        }
    }
}
