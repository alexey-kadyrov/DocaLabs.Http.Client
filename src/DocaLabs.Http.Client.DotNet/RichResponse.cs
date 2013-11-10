using System;
using System.Net;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Defines additional information about the web response.
    /// </summary>
    [Serializable]
    public abstract class RichResponse : RichResponseCore
    {
        /// <summary>
        /// Gets the value of the 'HttpWebResponse.LastModified or FtpWebResponse.LastModified and converts it to UTC.
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Returns whenever the current StatusCode equals to the specified FtpStatusCode.
        /// </summary>
        public bool Is(FtpStatusCode status)
        {
            return (int) status == StatusCode;
        }

        /// <summary>
        /// Initializes an instance of the RichResponse class by pulling additional information from WebResponse instance.
        /// </summary>
        protected RichResponse(WebResponse response)
            : base(response)
        {
            var httpResponse = response as HttpWebResponse;
            if (httpResponse != null)
            {
                LastModified = httpResponse.LastModified.ToUniversalTime();
                return;
            }

            var ftpResponse = response as FtpWebResponse;
            if (ftpResponse == null)
                return;

            StatusCode = (int) ftpResponse.StatusCode;
            StatusDescription = ftpResponse.StatusDescription;
            LastModified = ftpResponse.LastModified.ToUniversalTime();
        }
    }

    /// <summary>
    /// A generic that can be used to wrap the output model in order to get more information about the operation status.
    /// If you subclass that you must provide constructor with parameters (WebResponse response, object value).
    /// </summary>
    /// <typeparam name="T">Your output model.</typeparam>
    public class RichResponse<T> : RichResponse
    {
        /// <summary>
        /// Gets the value that is returned with the response.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the Response{} class by pulling additional information from WebResponse instance.
        /// </summary>
        public RichResponse(WebResponse response, object value)
            : base(response)
        {
            if (value != null)
                Value = (T)value;
        }
    }
}
