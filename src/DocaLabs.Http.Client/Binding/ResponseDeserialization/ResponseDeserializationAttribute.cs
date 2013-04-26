using System;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Defines base class for attributes that are used to deserialize objects from a web response stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class ResponseDeserializationAttribute : Attribute, IResponseDeserialization
    {
        /// <summary>
        /// When is overridden in derived class it deserializes an object from the web response.
        /// </summary>
        public abstract object Deserialize(HttpResponse response, Type resultType);
    }
}
