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
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
        void Serialize(object obj, WebRequest request);
    }
}