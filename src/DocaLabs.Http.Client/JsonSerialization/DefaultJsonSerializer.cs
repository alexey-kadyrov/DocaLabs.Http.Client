using System.Web.Script.Serialization;

namespace DocaLabs.Http.Client.JsonSerialization
{
    /// <summary>
    /// Implements IJsonSerializer using JavaScriptSerializer.
    /// All members are thread safe.
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
    }
}