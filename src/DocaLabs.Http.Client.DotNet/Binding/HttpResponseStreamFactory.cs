using System;
using System.Net;
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
        public HttpResponseStreamCore CreateStream(BindingContext context, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            request.Timeout = context.Configuration.RequestTimeout;

            return new HttpResponseStream(request.GetResponse(), context.Configuration.ReadTimeout);
        }

        /// <summary>
        /// Initializes an asynchronous instance of the HttpResponseStream class from the provided WebRequest instance.
        /// </summary>
        public async Task<HttpResponseStreamCore> CreateAsyncStream(AsyncBindingContext context, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            request.Timeout = context.Configuration.RequestTimeout;

            var response = await request.GetResponseAsync();

            return new HttpResponseStream(response, context.Configuration.ReadTimeout);
        }
    }
}
