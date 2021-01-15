using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public static class HttpRequestHandler
    {
        public static async Task<HttpResponseMessage> Process(HttpRequest request, ILogger logger, string id)
        {
            request.Headers.TryGetValue("Authorization", out StringValues authorizationHeaderValues);
            string authToken = authorizationHeaderValues.ToString().Split(' ')[1];

            try
            {
                if (request.Method == HttpMethod.Post.Method)
                {
                    return await HandlePostRequest(request, authToken, logger);
                }
                else if (request.Method == HttpMethod.Put.Method)
                {
                    return await HandlePutRequest(request, authToken, logger, id);
                }
                else
                {
                    return await HandleGetRequest(request, authToken, logger, id);
                }
            }
            catch (HttpRequestException error)
            {
                logger.LogError($"ERROR: Departments request failed with error {error.Message}");

                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { message = ConstantValues.DEPARTMENTS_REQUEST_FAILED }),
                                                Encoding.UTF8,
                                                "application/json")
                };
            }
        }

        private static async Task<HttpResponseMessage> HandleGetRequest(HttpRequest request, string token, ILogger log, string id)
        {
            string page = request.Query["page"];
            var queryPage = string.IsNullOrEmpty(page) ? string.Empty : $"?page={page}";
            var uri = string.IsNullOrEmpty(id) ? $"/jobpositions/{queryPage}" : $"/jobpositions/{id}/";

            var _httpService = new HttpService();
            var serviceResponse = await _httpService.Get(uri, token);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content,
                                                            Encoding.UTF8,
                                                            "application/json");

                log.LogInformation($"INFO: Job position(s) resource request successful");
            }
            else
            {
                log.LogError($"ERROR: Job position(s) resource request FAILED with response {serviceResponse.Content}");

                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new { message =  ConstantValues.JOB_POSITIONS_REQUEST_FAILED }),
                                                            Encoding.UTF8,
                                                            "application/json");
            }

            return responseMessage;
        }

        private static Task<HttpResponseMessage> HandlePutRequest(HttpRequest request, string token, ILogger log, string id)
        {
            throw new NotImplementedException();
        }

        private static Task<HttpResponseMessage> HandlePostRequest(HttpRequest request, string token, ILogger log)
        {
            throw new NotImplementedException();
        }
    }
}
