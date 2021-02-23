using Api.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Factories
{
    public class HttpPUTMethodHandler : IHttpMethodHandlerFactory
    {
        private readonly HttpService httpService;

        public HttpPUTMethodHandler()
        {
            httpService = new HttpService();
        }

        public async Task<HttpResponseMessage> HandleRequest(HttpRequest request, string authenticationToken, ILogger logger, string id = null)
        {
            var resource = request.GetResourceFromPath();
            var uri = $"/{resource}/{id}/";

            dynamic data = await request.GetRequestBody();

            string department = data?.department;
            string title = data?.title;
            int hierarchy_order = data?.hierarchy_order;

            var serviceResponse = await httpService.Put(uri, new { id, title, hierarchy_order, department}, authenticationToken);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content, Encoding.UTF8, "application/json");
                logger.LogInformation($"INFO: {resource} resource updated successfully");
            }
            else
            {
                logger.LogError($"ERROR: {resource } resource update request FAILED with response {serviceResponse.Content}");
                responseMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(new { message = ConstantValues.JOB_POSITIONS_UPDATE_FAILED }),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return responseMessage;
        }
    }
}
