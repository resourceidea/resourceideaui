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
using System.Net;
using System.Text;

namespace Api
{
    public static class JobPositions
    {
        [FunctionName("JobPositions")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = "jobpositions/{id?}")] HttpRequest request,
            string id,
            ILogger logger)
        {
            logger.LogInformation("Job positions resource request.");

            return await HttpRequestHandler.Process(request, logger, id);
        }
    }
}
