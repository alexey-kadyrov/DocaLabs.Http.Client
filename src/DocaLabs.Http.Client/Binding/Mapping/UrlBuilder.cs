using System;
using DocaLabs.Http.Client.Binding.Attributes;

namespace DocaLabs.Http.Client.Binding.Mapping
{
    /// <summary>
    /// Adds query string build from query object to the specified URL.
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        /// Adds query string build from query object to the specified URL.
        /// </summary>
        public static Uri CreateUrl(Uri serviceUrl, object query)
        {
            try
            {
                return new UriBuilder(serviceUrl)
                {
                    Query = TryMakeQueryString(query)
                }.Uri;
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(string.Format(Resources.Text.failed_create_url, serviceUrl), e);
            }
        }

        static string TryMakeQueryString(object query)
        {
            if (query == null)
                return "";

            return query.GetType().GetCustomAttributes(typeof(QueryIgnoreAttribute), true).Length == 0
                ? QueryMapper.ToQueryString(query)
                : "";
        }
    }
}
