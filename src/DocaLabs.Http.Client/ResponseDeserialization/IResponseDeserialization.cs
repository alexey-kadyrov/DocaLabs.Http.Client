using System;

namespace DocaLabs.Http.Client.ResponseDeserialization
{
    /// <summary>
    /// Defines methods that are used to deserialize objects from a web response stream.
    /// </summary>
    public interface IResponseDeserialization
    {
        /// <summary>
        /// When is overridden in derived class it deserializes an object from the web response.
        /// </summary>
        object Deserialize(HttpResponse response, Type resultType);
    }
}