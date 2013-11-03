using System;
using System.Collections.Concurrent;
using System.Web.Script.Serialization;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Extension.MSJavaScriptSerializer
{
    /// <summary>
    /// Implements IJsonSerializer using JavaScriptSerializer.
    /// All members are thread safe.
    /// </summary>
    public class MSJsonSerializer : IJsonSerializer
    {
        static readonly ConcurrentDictionary<Type, SerializationSettings> Settings = new ConcurrentDictionary<Type, SerializationSettings>();

        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            SerializationSettings settings;

            return obj != null && Settings.TryGetValue(obj.GetType(), out settings)
                ? new JavaScriptSerializer(settings.TypeResolver) { MaxJsonLength = settings.MaxJsonLength, RecursionLimit = settings.RecursionLimit }.Serialize(obj)
                : new JavaScriptSerializer().Serialize(obj);
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being serialized.
        /// Use that to customize behaviour of the JsonConvert.
        /// </summary>
        static public void UpdateSettings(Type type, SerializationSettings settings)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (settings == null)
                throw new ArgumentNullException("settings");

            Settings[type] = settings;
        }
    }
}