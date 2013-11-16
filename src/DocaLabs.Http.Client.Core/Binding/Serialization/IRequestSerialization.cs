using System.Net;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines methods to serialize an object into web request.
    /// </summary>
    public interface IRequestSerialization
    {
        /// <summary>
        /// When is overridden in derived class it serializes a given object into the web request.
        /// </summary>
        void Serialize(BindingContext context, WebRequest request, object value);
    }
}