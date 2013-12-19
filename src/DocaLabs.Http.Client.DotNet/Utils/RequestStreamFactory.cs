using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Implements IRequestStreamFactory for plain .Net environment to get a web request stream.
    /// </summary>
    public class RequestStreamFactory : IRequestStreamFactory
    {
        /// <summary>
        /// Gets the request stream for synchronous request.
        /// </summary>
        public Stream Get(BindingContext context, WebRequest request)
        {
            var stream = request.GetRequestStream();

            if (stream != null && stream.CanTimeout)
                stream.WriteTimeout = context.Configuration.WriteTimeout;

            return stream;
        }

        /// <summary>
        /// Gets the request stream for asynchronous request.
        /// </summary>
        public async Task<Stream> GetAsync(AsyncBindingContext context, WebRequest request)
        {
            var stream = await request.GetRequestStreamAsync();

            if (stream != null && stream.CanTimeout)
                stream.WriteTimeout = context.Configuration.WriteTimeout;

            return stream;
        }
    }
}
