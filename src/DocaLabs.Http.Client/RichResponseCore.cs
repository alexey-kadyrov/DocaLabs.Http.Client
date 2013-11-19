using System;
using System.Net;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Defines additional information about the web response.
    /// </summary>
    public abstract class RichResponseCore
    {
        /// <summary>
        /// Content type of the data being received.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public int StatusCode { get; protected set; }

        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        public string StatusDescription { get; protected set; }

        /// <summary>
        /// Gets the value of the 'ETag' response header if it's present.
        /// </summary>
        public string ETag { get; private set; }

        /// <summary>
        /// Gets a collection of header name-value pairs associated with the response.
        /// If the response doesn't support headers the collection will be empty.
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        /// <summary>
        /// Returns whenever the current StatusCode equals to the specified HttpStatusCode.
        /// </summary>
        public bool Is(HttpStatusCode status)
        {
            return (int) status == StatusCode;
        }

        /// <summary>
        /// Initializes an instance of the RichResponse class by pulling additional information from WebResponse instance.
        /// </summary>
        protected RichResponseCore(WebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            Headers = new WebHeaderCollection();

            ContentType = response.ContentType;

            if (response.SupportsHeaders)
            {
                foreach (var key in response.Headers.AllKeys)
                    Headers[key] = response.Headers[key];

                ETag = response.Headers["ETag"];
            }

            var httpResponse = response as HttpWebResponse;
            if (httpResponse == null)
                return;

            StatusCode = (int)httpResponse.StatusCode;
            StatusDescription = httpResponse.StatusDescription;
        }
    }
}
