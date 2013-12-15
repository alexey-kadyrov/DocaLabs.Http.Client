using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Implements IJsonDeserializer using DataContractJsonSerializer.
    /// All members are thread safe.
    /// </summary>
    public class DefaultJsonDeserializer : IJsonDeserializer
    {
        static readonly ICustomConcurrentDictionary<Type, DataContractJsonSerializerSettings> Settings = new CustomConcurrentDictionary<Type, DataContractJsonSerializerSettings>();

        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            if (string.IsNullOrWhiteSpace(value))
                return null;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                DataContractJsonSerializerSettings settings;

                var serializer = Settings.TryGetValue(resultType, out settings)
                    ? new DataContractJsonSerializer(resultType, settings)
                    : new DataContractJsonSerializer(resultType);

                return serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being deserialized.
        /// Use that to customize behaviour of the JsonConvert.
        /// </summary>
        static public void UpdateSettings(Type type, DataContractJsonSerializerSettings settings)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (settings == null)
                throw new ArgumentNullException("settings");

            Settings[type] = settings;
        }
    }
}