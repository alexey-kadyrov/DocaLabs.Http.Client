using System;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Newtonsoft.Json;

namespace DocaLabs.Extensions.Http.Client.NewtonSoft
{
    /// <summary>
    /// Implements IJsonDeserializer using Newtonsoft JsonConvert.
    /// </summary>
    [Export(typeof(IJsonDeserializer))]
    public class NewtonSoftJsonDeserializer : IJsonDeserializer
    {
        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }
    }
}