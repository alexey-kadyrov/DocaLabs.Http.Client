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
                return CreateUrlFrom(model, client, baseUrl);
            }
            catch (UnrecoverableHttpClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(string.Format(Resources.Text.failed_create_url, baseUrl), e);
            }
        }

        static Uri CreateUrlFrom(object model, object client, Uri baseUrl)
        {
            var builder = new UriBuilder(GetBaseUrl(baseUrl))
            {
                Path = new PathMapper(model, client, baseUrl).TryMakePath(),
                Query = new QueryMapper(model, client, baseUrl).TryMakeQuery(),
                Fragment = GetFragmentWithoutSharpMark(baseUrl)
            };

            return builder.Uri;
        }

        static string GetBaseUrl(Uri baseUrl)
        {
            return baseUrl.IsFile || baseUrl.IsUnc 
                ? baseUrl.ToString() 
                : baseUrl.GetLeftPart(UriPartial.Authority);
        }

        static string GetFragmentWithoutSharpMark(Uri url)
        {
            var fragment = url.Fragment;

            return fragment.StartsWith("#") 
                ? fragment.Substring(1) 
                : fragment;
        }
    }
}
