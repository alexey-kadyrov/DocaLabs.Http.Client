using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Implements IJsonSerializer using DataContractJsonSerializer.
    /// All members are thread safe.
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        static readonly ICustomConcurrentDictionary<Type, DataContractJsonSerializerSettings> Settings = new CustomConcurrentDictionary<Type, DataContractJsonSerializerSettings>();

        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            if (obj == null)
                return "";

            var type = obj.GetType();

            using (var stream = new MemoryStream())
            {
                DataContractJsonSerializerSettings settings;

                var serializer = Settings.TryGetValue(obj.GetType(), out settings)
                    ? new DataContractJsonSerializer(type, settings)
                    : new DataContractJsonSerializer(type);

                serializer.WriteObject(stream, obj);

                var data = stream.ToArray();

                return Encoding.UTF8.GetString(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being serialized.
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