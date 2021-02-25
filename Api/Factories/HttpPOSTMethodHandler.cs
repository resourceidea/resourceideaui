using Api.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Factories
{
    public class HttpPOSTMethodHandler : IHttpMethodHandlerFactory
    {
        private readonly HttpService httpService;

        public HttpPOSTMethodHandler()
        {
            httpService = new HttpService();
        }

        public async Task<HttpResponseMessage> HandleRequest(HttpRequest request, string authenticationToken, ILogger logger, string id = null)
        {
            var resource = request.GetResourceFromPath();
            var uri = $"/{resource}/";

            dynamic data = await request.GetRequestBody();

            string department = data?.department;
            string title = data?.title;
            int hierarchy_order = data?.hierarchy_order;

            var serviceResponse = await httpService.Post(uri, new { department, title, hierarchy_order}, authenticationToken);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.Created)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content, Encoding.UTF8, "application/json");
                logger.LogInformation($"INFO: {resource} resource created successfully");
            }
            else
            {
                logger.LogError($"ERROR: {resource } resource create request FAILED with response {serviceResponse.Content}");
                responseMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(new { message = ConstantValues.JOB_POSITIONS_CREATE_FAILED }),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return responseMessage;
        }
    }
}
