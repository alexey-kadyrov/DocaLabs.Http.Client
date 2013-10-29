using System;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Defines methods to deserialize an object from string in JSON notation.
    /// Note for implementation: all members must be thread safe.
    /// </summary>
    public interface IJsonDeserializer
    {
        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        object Deserialize(string value, Type resultType);
    }
}
