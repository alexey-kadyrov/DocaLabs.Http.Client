using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines methods that are used to asynchronously deserialize objects from a web response stream.
    /// </summary>
    public interface IAsyncResponseDeserialization
    {
        /// <summary>
        /// When is overridden in derived class it asynchronously deserializes an object from the web response stream.
        /// </summary>
        Task<object> DeserializeAsync(HttpResponseStream responseStream, Type resultType, CancellationToken cancellationToken);
    }
}