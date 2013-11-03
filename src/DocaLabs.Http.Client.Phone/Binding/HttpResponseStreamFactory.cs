using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Creates instances of the HttpResponseStream class.
    /// </summary>
    public class HttpResponseStreamFactory : IHttpResponseStreamFactory
    {
        /// <summary>
        /// Initializes an instance of the HttpResponseStream class from the provided WebRequest instance.
        /// </summary>
        public IHttpResponseStream CreateStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var response = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null).Result;

            return new HttpResponseStream(response);
        }

        /// <summary>
        /// Initializes an asynchronous instance of the HttpResponseStream class from the provided WebRequest instance.
        /// </summary>
        public async Task<IHttpResponseStream> CreateAsyncStream(WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);

            return new HttpResponseStream(response);
        }
    }
}
