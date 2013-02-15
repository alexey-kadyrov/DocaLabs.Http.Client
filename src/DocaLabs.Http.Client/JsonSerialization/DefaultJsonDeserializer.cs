using System;
using System.Web.Script.Serialization;

namespace DocaLabs.Http.Client.JsonSerialization
{
    /// <summary>
    /// Implements IJsonDeserializer using JavaScriptSerializer.
    /// All members are thread safe.
    /// </summary>
    public class DefaultJsonDeserializer : IJsonDeserializer
    {
        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type type)
        {
            return new JavaScriptSerializer().Deserialize(value, type);
        }
    }
}