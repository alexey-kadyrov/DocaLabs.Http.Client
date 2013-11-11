using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines base class for attributes that are used to deserialize objects from a web response stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class ResponseDeserializationAttribute : Attribute, IResponseDeserialization, IAsyncResponseDeserialization
    {
        /// <summary>
        /// When is overridden in derived class it deserializes an object from the web response stream.
        /// </summary>
        public abstract object Deserialize(HttpResponseStreamCore responseStream, Type resultType);

        /// <summary>
        /// When is overridden in derived class it asynchronously deserializes an object from the web response stream.
        /// The default implementation creates a Task using Task.FromResult that's completed successfully with the result of the Deserialize.
        /// </summary>
        public virtual Task<object> DeserializeAsync(HttpResponseStreamCore responseStream, Type resultType, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(Deserialize(responseStream, resultType));
        }
    }
}
