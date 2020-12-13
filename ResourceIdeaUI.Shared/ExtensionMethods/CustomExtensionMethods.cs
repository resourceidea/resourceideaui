using System;
namespace ResourceIdeaUI.Shared.ExtensionMethods
{
    public static class CustomExtensionMethods
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
    }
}
