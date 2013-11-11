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
        public HttpResponseStreamCore CreateStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return new HttpResponseStream(request.GetResponse());
        }

        /// <summary>
        /// Initializes an asynchronous instance of the HttpResponseStream class from the provided WebRequest instance.
        /// </summary>
        public async Task<HttpResponseStreamCore> CreateAsyncStream(WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var response = await request.GetResponseAsync();

            return new HttpResponseStream(response);
        }
    }
}
