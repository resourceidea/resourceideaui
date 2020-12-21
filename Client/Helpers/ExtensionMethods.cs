using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Client.Helpers
{
    public static class ExtensionMethods
    {
        private const string DEFAULT_START_PAGE_NUMBER = "1";

        public static string GetPageNumber(this string str)
        {
            var pageNumber = string.Empty;
            if (str != null && !string.IsNullOrEmpty(str))
            {
                if (str.EndsWith("/"))
                {
                    pageNumber = DEFAULT_START_PAGE_NUMBER;
                }
                else
                {
                    var urlParts = str?.Split('?');
                    if (urlParts.Length > 1)
                    {
                        var queryParts = urlParts[1].Split('=');
                        if (queryParts.Length > 1)
                        {
                            pageNumber = queryParts[1];
                        }
                    }
                }
            }

            return pageNumber;
        }
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }
    }
}
