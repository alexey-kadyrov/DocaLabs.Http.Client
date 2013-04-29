using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding.Utils
{
    /// <summary>
    /// WebRequest extensions.
    /// </summary>
    public static class WebRequestExtensions
    {
        /// <summary>
        /// Sets the ContentLength to zero if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public static void SetContentLengthToZeroIfBodyIsRequired(this WebRequest request)
        {
            if (IsBodyRequired(request))
                request.ContentLength = 0;
        }

        /// <summary>
        /// Returns true if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public static bool IsBodyRequired(this WebRequest request)
        {
            return
                (
                    request is HttpWebRequest || request is FileWebRequest
                ) &&
                (
                    string.Compare(request.Method, WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                    string.Compare(request.Method, WebRequestMethods.Http.Put, StringComparison.InvariantCultureIgnoreCase) == 0
                );
        }
    }
}
