using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines base class for attributes that are used to serialize objects into a web request stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class RequestSerializationAttribute : Attribute, IRequestSerialization, IAsyncRequestSerialization
    {
        /// <summary>
        /// When is overridden in derived class it serializes a given object into the web request.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
        public abstract void Serialize(object obj, WebRequest request);

        /// <summary>
        /// When is overridden in derived class it asynchronously serializes a given object into the web request.
        /// </summary>
        public virtual Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => Serialize(obj, request), cancellationToken);
        }
    }
}
