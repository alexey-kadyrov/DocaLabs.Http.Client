using System;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;

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
        public abstract void Serialize(BindingContext context, WebRequest request, object value);

        /// <summary>
        /// When is overridden in derived class it asynchronously serializes a given object into the web request.
        /// </summary>
        public virtual Task SerializeAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            return TaskUtils.RunSynchronously(() => Serialize(context, request, value), context.CancellationToken);
        }
    }
}
