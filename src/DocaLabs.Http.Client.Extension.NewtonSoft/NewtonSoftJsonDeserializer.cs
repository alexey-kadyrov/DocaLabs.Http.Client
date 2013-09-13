using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft
{
    /// <summary>
    /// Implements IJsonDeserializer using Newtonsoft JsonConvert.
    /// </summary>
    [Export(typeof(IJsonDeserializer))]
    public class NewtonSoftJsonDeserializer : IJsonDeserializer
    {
        static readonly ConcurrentDictionary<Type, JsonSerializerSettings> Settings = new ConcurrentDictionary<Type, JsonSerializerSettings>();

        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type resultType)
        {
            JsonSerializerSettings settings;

            return resultType != null && Settings.TryGetValue(resultType, out settings)
                ? JsonConvert.DeserializeObject(value, resultType, settings)
                : JsonConvert.DeserializeObject(value, resultType);
        }

        /// <summary>
        /// Updates/adds settings information whoich will be used when the specified type is being serialized.
        /// Use that to customize behaviour of the JsonConvert.
        /// </summary>
        static public void UpdateSettings(Type type, JsonSerializerSettings setting)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Settings[type] = setting;
        }
    }
}