using System.Net;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// A generic that can be used to wrap the output model in order to get more information about the operation status.
    /// If you subclass that you must provide constructor with parameters (int statusCode, string statusDescription, object value).
    /// </summary>
    /// <typeparam name="T">Your output model.</typeparam>
    public class RichResponse<T>
    {
        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets the value return with the response.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the Response{} class with specified values for all properties.
        /// </summary>
        public RichResponse(int statusCode, string statusDescription, object value)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;

            if (value != null)
                Value = (T) value;
        }

        /// <summary>
        /// Returns whenever the current StatusCode equals to the specified HttpStatusCode.
        /// </summary>
        public bool Is(HttpStatusCode status)
        {
            return (int) status == StatusCode;
        }

        /// <summary>
        /// Returns whenever the current StatusCode equals to the specified FtpStatusCode.
        /// </summary>
        public bool Is(FtpStatusCode status)
        {
            return (int)status == StatusCode;
        }
    }
}
