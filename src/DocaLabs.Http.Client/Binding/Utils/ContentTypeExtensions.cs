using System;
using System.Net.Mime;

namespace DocaLabs.Http.Client.Binding.Utils
{
    /// <summary>
    /// Content type extensions.
    /// </summary>
    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Returns true if the content type is of the specified mime type.
        /// Always returns false if either contentType or mediaType are null.
        /// </summary>
        public static bool Is(this ContentType contentType, string mediaType)
        {
            if (contentType == null || mediaType == null)
                return false;

            return string.Compare(contentType.MediaType, mediaType, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
