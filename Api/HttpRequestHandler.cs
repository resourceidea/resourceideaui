using Api.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
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
            try
            {
                request.Headers.TryGetValue("Authorization", out StringValues authorizationHeaderValues);
                string authenticationToken = authorizationHeaderValues.ToString().Split(' ')[1];

                var httpMethodHandler = $"Api.Factories.Http{request.Method}MethodHandler, Api";
                var httpMethodHandlerFactory = Activator.CreateInstance(
                                                            Type.GetType(httpMethodHandler) ?? throw new InvalidOperationException()
                                                        ) as IHttpMethodHandlerFactory;

                return await httpMethodHandlerFactory?.HandleRequest(request, authenticationToken, logger, id);
            }
            catch (HttpRequestException error)
            {
                logger.LogError($"ERROR: Request failed with error {error.Message}");

                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new { 
                            message = ConstantValues.DEPARTMENTS_REQUEST_FAILED 
                        }),
                        Encoding.UTF8,
                        "application/json")
                };
            }
        }
    }
}
