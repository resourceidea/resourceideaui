using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Api.ExtensionMethods
{
    public static class HttpRequestExtensions
    {
        public static string GetPageFromQueryString(this HttpRequest httpRequest)
        {
            return httpRequest.Query["page"];
        }

        public static string GetResourceFromPath(this HttpRequest httpRequest)
        {
            return httpRequest.Path.Value.Split('/')[2];
        }

        public static async Task<dynamic> GetRequestBody(this HttpRequest httpRequest)
        {
            string requestBody = await new StreamReader(httpRequest.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return data;
        }
    }
}
