using System;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines base class for attributes that are used to deserialize objects from a web response stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class ResponseDeserializationAttribute : Attribute, IResponseDeserialization
    {
        /// <summary>
        /// When is overridden in derived class it deserializes an object from the web response stream.
        /// </summary>
        public abstract object Deserialize(HttpResponseStream responseStream, Type resultType);
    }
}
