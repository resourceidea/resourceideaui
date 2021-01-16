using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
