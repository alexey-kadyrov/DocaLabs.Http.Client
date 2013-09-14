using System;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft
{
    /// <summary>
    /// Defines serialization settings that can be used to customize the behaviour of the NewtonSoftDefaultJsonSerializer class.
    /// </summary>
    public class SerializationSettings
    {
        /// <summary>
        /// The type of the value being serialized. This parameter is used when Newtonsoft.Json.TypeNameHandling" is Auto to write out the type name if the type of the value does not match.
        /// Specifying the type is optional. The default value is null.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Indicates how the output is formatted.
        /// The default value is None.
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// The settings used to serialize the object. If this is null, default serialization settings will be is used.
        /// The default value is null.
        /// </summary>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Initializes an instance of the SerializationSettings class with default values.
        /// </summary>
        public SerializationSettings()
        {
        }

        /// <summary>
        /// Initializes an instance of the SerializationSettings class with the specified values.
        /// </summary>
        public SerializationSettings(Type type, Formatting formatting, JsonSerializerSettings setting)
        {
            Type = type;
            Formatting = formatting;
            Settings = setting;
        }
    }
}
