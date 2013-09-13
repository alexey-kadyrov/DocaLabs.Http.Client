using System;
using System.Collections.Concurrent;
using System.Web.Script.Serialization;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Implements IJsonDeserializer using JavaScriptSerializer.
    /// All members are thread safe.
    /// </summary>
    public class DefaultJsonDeserializer : IJsonDeserializer
    {
        static readonly ConcurrentDictionary<Type, SerializationSettings> Settings = new ConcurrentDictionary<Type, SerializationSettings>();

        /// <summary>
        /// Deserializes an object from string in JSON notation.
        /// </summary>
        public object Deserialize(string value, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            SerializationSettings settings;

            return Settings.TryGetValue(resultType, out settings)
                ? new JavaScriptSerializer(settings.TypeResolver) { MaxJsonLength = settings.MaxJsonLength, RecursionLimit = settings.RecursionLimit }.Deserialize(value, resultType)
                : new JavaScriptSerializer().Deserialize(value, resultType);
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being deserialized.
        /// Use that to customize behaviour of the JsonConvert.
        /// </summary>
        static public void UpdateSettings(Type type, SerializationSettings setting)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (setting == null)
                throw new ArgumentNullException("setting");

            Settings[type] = setting;
        }
    }
}