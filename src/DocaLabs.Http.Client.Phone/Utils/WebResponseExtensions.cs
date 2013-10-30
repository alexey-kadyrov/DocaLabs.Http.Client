using System.Net;

namespace DocaLabs.Http.Client.Utils
{
    public static class WebResponseExtensions
    {
        const string ContentEncodingHeader = "Content-Encoding";

        public static string GetContentEncoding(this WebResponse response)
        {
            return response.SupportsHeaders 
                ? (response.Headers[ContentEncodingHeader] ?? string.Empty) 
                : string.Empty;
        }
    }
}
