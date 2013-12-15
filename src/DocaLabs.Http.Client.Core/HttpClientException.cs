using System;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// The exception that is thrown when errors occur during request execution.
    /// </summary>
    public class HttpClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HttpClientException class.
        /// </summary>
        public HttpClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpClientException class with a specified error message.
        /// </summary>
        public HttpClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpClientException class with a specified error message and a reference to the inner exception that caused the current exception.
        /// </summary>
        public HttpClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
