using System;
using System.Net;
using System.Text;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// The exception that is thrown when WebExceotion occurs during request execution.
    /// </summary>
    public class HttpClientWebException : HttpClientException
    {
        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public WebExceptionStatus Status { get; private set; }

        /// <summary>
        /// Gets additional information about the exception if the inner exception was of WebException.
        /// </summary>
        public ResponseInfo Response { get; private set; }

        /// <summary>
        /// Content type of the data being received.
        /// </summary>
        public string ContentType { get { return Response != null ? Response.ContentType : ""; } }

        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public int StatusCode { get { return Response != null ? Response.StatusCode : -1; } }

        /// <summary>
        /// Gets the status description returned with the response.
        /// </summary>
        public string StatusDescription { get { return Response != null ? Response.StatusDescription : ""; } }

        /// <summary>
        /// Gets the value of the 'ETag' response header if it's present.
        /// </summary>
        public string ETag { get { return Response != null ? Response.ETag : ""; } }

        /// <summary>
        /// Gets a collection of header name-value pairs associated with the response.
        /// If the response doesn't support headers the collection will be empty.
        /// </summary>
        public WebHeaderCollection Headers { get { return Response != null ? Response.Headers : new WebHeaderCollection(); } }

        /// <summary>
        /// Initializes a new instance of the HttpClientException class with a specified error message and a reference to the inner exception that caused the current exception.
        /// If the inner is WebException then it'll initialize the Request property.
        /// </summary>
        public HttpClientWebException(string message, Exception innerException)
            : base(message, innerException)
        {
            var webException = innerException as WebException;
            if (webException != null)
            {
                if(webException.Response != null)
                    Response = new ResponseInfo(webException.Response);

                Status = webException.Status;
            }
            else
            {
                Status = WebExceptionStatus.UnknownError;
            }
        }

        /// <summary>
        /// COntains additional information about the response.
        /// </summary>
        public class ResponseInfo : RichResponseCore
        {
            /// <summary>
            /// Initializes an instance of the ResponseInfo class.
            /// </summary>
            /// <param name="response"></param>
            public ResponseInfo(WebResponse response)
                : base(response)
            {
            }
        }

        /// <summary>
        /// Returns a string that represents the information of current exception.
        /// </summary>
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Response != null)
            {
                builder.Append("HttpClientWebException: ").AppendLine(Message)
                       .Append("StatusCode: ").Append(Response.StatusCode).AppendLine()
                       .Append("StatusDescription: ").AppendLine(Response.StatusDescription)
                       .Append("ETag: ").AppendLine(Response.ETag)
                       .AppendLine("Headers:");

                foreach (var key in Response.Headers.AllKeys)
                    builder.Append(key).Append(": ").AppendLine(Response.Headers[key]);

                builder.AppendLine()
                       .AppendLine("Exception:");
            }

            return builder.Append(base.ToString()).ToString();
        }
    }
}
