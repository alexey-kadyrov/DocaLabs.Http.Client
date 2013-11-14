using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines methods to create instances of classes that implement HttpResponseStreamCore.
    /// </summary>
    public interface IHttpResponseStreamFactory
    {
        /// <summary>
        /// Initializes the response stream from the provided WebRequest instance.
        /// </summary>
        HttpResponseStreamCore CreateStream(BindingContext context, WebRequest request);

        /// <summary>
        /// Initializes an asynchronous instance of the response stream from the provided WebRequest instance.
        /// </summary>
        Task<HttpResponseStreamCore> CreateAsyncStream(AsyncBindingContext context, WebRequest request);
    }
}
