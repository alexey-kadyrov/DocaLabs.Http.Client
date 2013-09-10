using System;
using System.Net;
using System.Runtime.Serialization;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// The exception that is thrown when errors occur during request execution.
    /// </summary>
    [Serializable]
    public class HttpClientException : Exception
    {
        /// <summary>
        /// Gets additional information about the exception if the inner exception was of WebException.
        /// </summary>
        public ResponseInfo Response { get; private set; }

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
        /// If the inner is WebException then it'll initialize the Request property.
        /// </summary>
        public HttpClientException(string message, Exception innerException)
            : base(message, innerException)
        {
            var webException = innerException as WebException;
            if(webException != null)
                Response = new ResponseInfo(webException.Response);
        }

        /// <summary>
        /// Initializes a new instance of the HttpClientException class with serialized data. 
        /// </summary>
        protected HttpClientException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        /// <summary>
        /// COntains additional information about the response.
        /// </summary>
        public class ResponseInfo : RichResponse
        {
            /// <summary>
            /// Initializes an instance of the ResponseInfo class.
            /// </summary>
            /// <param name="response"></param>
            public ResponseInfo(WebResponse response) : base(response)
            {
            }
        }
    }
}
