using System;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Content type extensions.
    /// </summary>
    public static class HttpContentTypeExtensions
    {
        /// <summary>
        /// Returns true if the content type is of the specified mime type.
        /// Always returns false if either contentType or mediaType are null.
        /// </summary>
        public static bool Is(this HttpContentType contentType, string mediaType)
        {
            if (contentType == null || mediaType == null)
                return false;

            return string.Compare(contentType.MediaType, mediaType, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
