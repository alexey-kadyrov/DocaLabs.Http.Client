using System;
using System.Runtime.Serialization;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// The exception which is considered to be unrecoverable during request execution so retry should be carried out.
    /// </summary>
    [Serializable]
    public class UnrecoverableHttpClientException : HttpClientException
    {
        /// <summary>
        /// Initializes a new instance of the UnrecoverableHttpClientException class.
        /// </summary>
        public UnrecoverableHttpClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the UnrecoverableHttpClientException class with a specified error message.
        /// </summary>
        public UnrecoverableHttpClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the UnrecoverableHttpClientException class with a specified error message and a reference to the inner exception that caused the current exception.
        /// </summary>
        public UnrecoverableHttpClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the UnrecoverableHttpClientException class with serialized data. 
        /// </summary>
        protected UnrecoverableHttpClientException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
