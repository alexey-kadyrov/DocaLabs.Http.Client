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

            var response = GetResponseAsync(request, context.Configuration.Timeout).Result;

            return new HttpResponseStream(response, context.Configuration.ReadTimeout);
        }

        /// <summary>
        /// Initializes an asynchronous instance of the HttpResponseStream class from the provided WebRequest instance.
        /// </summary>
        public async Task<HttpResponseStreamCore> CreateAsyncStream(AsyncBindingContext context, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var response = await GetResponseAsync(request, context.Configuration.Timeout);

            return new HttpResponseStream(response, context.Configuration.ReadTimeout);
        }

        static async Task<WebResponse> GetResponseAsync(WebRequest request, int timeout)
        {
            var getResponseTask = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);

            var completeTask = await Task.WhenAny(getResponseTask, Task.Delay(timeout));
            if (completeTask != getResponseTask)
                throw new WebException(string.Format(Resources.Text.timeout_executing_request_to_0, request.RequestUri), WebExceptionStatus.ConnectFailure);

            return await getResponseTask;
        }
    }
}
