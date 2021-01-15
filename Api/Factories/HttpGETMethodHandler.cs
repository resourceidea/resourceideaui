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
    public class HttpGETMethodHandler : IHttpMethodHandlerFactory
    {
        public async Task<HttpResponseMessage> HandleRequest(HttpRequest request, string authenticationToken, ILogger logger, string id = null)
        {
            string resource = request.Path.Value.Split('/')[2];
            string page = request.Query["page"];

            var queryPage = string.IsNullOrEmpty(page) ? string.Empty : $"?page={page}";
            var uri = string.IsNullOrEmpty(id) ? $"/{resource}/{queryPage}" : $"/{resource}/{id}/";

            var _httpService = new HttpService();
            var serviceResponse = await _httpService.Get(uri, authenticationToken);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content, Encoding.UTF8, "application/json");
                logger.LogInformation($"INFO: Job position(s) resource request successful");
            }
            else
            {
                logger.LogError($"ERROR: Job position(s) resource request FAILED with response {serviceResponse.Content}");
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(new { message = ConstantValues.JOB_POSITIONS_REQUEST_FAILED }), Encoding.UTF8, "application/json");
            }

            return responseMessage;
        }
    }
}
