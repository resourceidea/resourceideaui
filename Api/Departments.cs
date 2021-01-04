using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text;
using System.Net;

namespace Api
{
    public static class Departments
    {
        [FunctionName("Departments")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = "departments/{id?}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            logger.LogInformation("INFO: Departments resource request");

            request.Headers.TryGetValue("Authorization", out StringValues authorizationHeaderValues);
            string authToken = authorizationHeaderValues.ToString().Split(' ')[1];
            try
            {
                return await RequestProcessor(request, id, logger, authToken);
            }
            catch (HttpRequestException error)
            {
                logger.LogError($"ERROR: Departments request failed with error {error.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new { message = "Department(s) resource request failed" }),
                                                 Encoding.UTF8,
                                                 "application/json")
                };
            }
        }

        private static async Task<HttpResponseMessage> RequestProcessor(HttpRequest req, string id, ILogger log, string authToken)
        {
            if (req.Method == HttpMethod.Post.Method)
            {
                return await HandlePostRequest(req, authToken, log);
            }
            else if (req.Method == HttpMethod.Put.Method)
            {
                return await HandlePutRequest(req, id);
            }
            else
            {
                return await HandleGetRequest(req, authToken, log);
            }
        }

        private static async Task<HttpResponseMessage> HandleGetRequest(HttpRequest request, string token, ILogger log)
        {
            string page = request.Query["page"];
            var queryPage = string.IsNullOrEmpty(page) ? string.Empty : $"?page={page}";

            var _httpService = new HttpService();
            var serviceResponse = await _httpService.Get($"/departments/{queryPage}", token);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content,
                                                            Encoding.UTF8,
                                                            "application/json");

                log.LogInformation($"INFO: Department(s) resource request successful");
            }
            else
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new { message = "Department(s) resource request failed" }),
                                                            Encoding.UTF8,
                                                            "application/json");

                log.LogError($"ERROR: Department(s) resource request FAILED with response {serviceResponse.Content}");
            }

            return responseMessage;
        }

        private static async Task<HttpResponseMessage> HandlePutRequest(HttpRequest request, string id)
        {
            throw new NotImplementedException();
        }

        private static async Task<HttpResponseMessage> HandlePostRequest(HttpRequest request, string token, ILogger log)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;
            string organization = data?.organization;
            var _httpService = new HttpService();
            var serviceResponse = await _httpService.Post("/departments/", new { name, organization }, token);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.Created)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content,
                                                            Encoding.UTF8,
                                                            "application/json");

                log.LogInformation($"INFO: Department added successfully");
            }
            else
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new { message = "Request to add new department failed" }),
                                                            Encoding.UTF8,
                                                            "application/json");

                log.LogError($"ERROR: Request to add department FAILED with response {serviceResponse.Content}");
            }

            return responseMessage;
        }
    }
}
