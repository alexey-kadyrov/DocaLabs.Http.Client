using System.Net;

namespace DocaLabs.Http.Client.Utils
{
    public static class WebResponseExtensions
    {
        const string ContentEncodingHeader = StandardHeaders.ContentEncoding;

        public static string GetContentEncoding(this WebResponse response)
        {
            return response.SupportsHeaders 
                ? (response.Headers[ContentEncodingHeader] ?? string.Empty) 
                : string.Empty;
        }
    }
}
