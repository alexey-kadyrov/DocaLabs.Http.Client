using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines the methods that are required to write asynchronously to the request body.
    /// </summary>
    public interface IAsyncRequestWriter
    {
        /// <summary>
        /// The method is called to write data asynchronously to the request's stream. It's expected that the method will set correctly
        /// property related to the data in the request, like content encoding, content length, etc.
        /// </summary>
        Task WriteAsync(AsyncBindingContext context, WebRequest request);
    }
}
