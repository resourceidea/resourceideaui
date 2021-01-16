using Api.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Factories
{
    public class HttpGETMethodHandler : IHttpMethodHandlerFactory
    {
        private readonly HttpService _httpService;

        public HttpGETMethodHandler()
        {
            _httpService = new HttpService();
        }

        public async Task<HttpResponseMessage> HandleRequest(HttpRequest request, string authenticationToken, ILogger logger, string id = null)
        {
            var resource = request.GetResourceFromPath();
            var page = request.GetPageFromQueryString();

            var queryPage = string.IsNullOrEmpty(page) ? string.Empty : $"?page={page}";
            var uri = string.IsNullOrEmpty(id) ? $"/{resource}/{queryPage}" : $"/{resource}/{id}/";

            var serviceResponse = await _httpService.Get(uri, authenticationToken);

            HttpResponseMessage responseMessage = new HttpResponseMessage(serviceResponse.StatusCode);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                responseMessage.Content = new StringContent(serviceResponse.Content, Encoding.UTF8, "application/json");
                logger.LogInformation($"INFO: {resource} resource request successful");
            }
            else
            {
                logger.LogError($"ERROR: {resource } resource request FAILED with response {serviceResponse.Content}");
                responseMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(new { message = ConstantValues.JOB_POSITIONS_REQUEST_FAILED }),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return responseMessage;
        }
    }
}
