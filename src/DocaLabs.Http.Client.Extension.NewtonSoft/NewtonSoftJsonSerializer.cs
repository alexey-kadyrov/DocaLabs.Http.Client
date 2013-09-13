using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft
{
    /// <summary>
    /// Implements IJsonSerializer using Newtonsoft JsonConvert.
    /// </summary>
    [Export(typeof(IJsonSerializer))]
    public class NewtonSoftDefaultJsonSerializer : IJsonSerializer
    {
        static readonly ConcurrentDictionary<Type, SerializationSettings> Settings = new ConcurrentDictionary<Type, SerializationSettings>();

        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            SerializationSettings settings;

            return obj != null && Settings.TryGetValue(obj.GetType(), out settings) 
                ? JsonConvert.SerializeObject(obj, settings.Type, settings.Formatting, settings.Settings) 
                : JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Updates/adds settings information which will be used when the specified type is being serialized.
        /// Use that to customize behaviour of the JsonConvert.
        /// </summary>
        static public void UpdateSettings(Type type, SerializationSettings setting)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(setting == null)
                throw new ArgumentNullException("setting");

            Settings[type] = setting;
        }
    }
}