using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Defines methods to get a web request stream.
    /// </summary>
    public interface IRequestStreamFactory
    {
        /// <summary>
        /// Gets the request stream for synchronous request.
        /// </summary>
        Stream Get(BindingContext context, WebRequest request);

        /// <summary>
        /// Gets the request stream for asynchronous request.
        /// </summary>
        Task<Stream> GetAsync(AsyncBindingContext context, WebRequest request);
    }
}
