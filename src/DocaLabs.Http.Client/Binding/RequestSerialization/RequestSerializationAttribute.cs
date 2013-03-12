using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding.RequestSerialization
{
    /// <summary>
    /// Defines base class for attributes that are used to serialize objects into a web request stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public abstract class RequestSerializationAttribute : Attribute, IRequestSerialization
    {
        /// <summary>
        /// When is overridden in derived class it serializes a given object into the web request.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
        public abstract void Serialize(object obj, WebRequest request);
    }
}
