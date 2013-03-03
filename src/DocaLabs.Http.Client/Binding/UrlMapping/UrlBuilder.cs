using System;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    /// <summary>
    /// Adds query string build from query object to the specified URL.
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        /// Adds query string build from query object to the specified URL.
        /// </summary>
        public static Uri CreateUrl(object model, object client, Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException("baseUrl");

            try
            {
                var builder =  new UriBuilder(baseUrl.GetLeftPart(UriPartial.Authority))
                {
                    Path = new PathMapper(model, client, baseUrl).TryMakePath(),
                    Query = new QueryMapper(model, client, baseUrl).TryMakeQuery()
                };
                
                return builder.Uri;
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(string.Format(Resources.Text.failed_create_url, baseUrl), e);
            }
        }
    }
}
