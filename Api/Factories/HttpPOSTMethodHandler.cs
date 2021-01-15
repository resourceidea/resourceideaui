using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Factories
{
    public class HttpPOSTMethodHandler : IHttpMethodHandlerFactory
    {
        public Task<HttpResponseMessage> HandleRequest(HttpRequest request, string authenticationToken, ILogger logger, string id = null)
        {
            throw new NotImplementedException();
        }
    }
}
