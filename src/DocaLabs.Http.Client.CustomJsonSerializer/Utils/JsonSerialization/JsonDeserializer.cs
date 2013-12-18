using System;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Implements IJsonDeserializer using Newtonsoft JsonConvert.
    /// </summary>
    public class JsonDeserializer : IJsonDeserializer
    {
        static readonly CustomConcurrentDictionary<Type, JsonSerializerSettings> Settings = new CustomConcurrentDictionary<Type, JsonSerializerSettings>();

        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            JsonSerializerSettings settings;

            return Settings.TryGetValue(resultType, out settings)
                ? JsonConvert.DeserializeObject(value, resultType, settings)
                : JsonConvert.DeserializeObject(value, resultType);
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being serialized.
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