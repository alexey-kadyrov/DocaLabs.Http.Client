using System.Web.Script.Serialization;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Defines serialization settings that can be used to customize the bahaviour of the JavaScriptSerializer class.
    /// </summary>
    public class SerializationSettings
    {
        /// <summary>
        /// Gets or stes an instance of the JavaScriptSerializer class that has a custom type resolver.
        /// The default value is null.
        /// </summary>
        public JavaScriptTypeResolver TypeResolver { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of JSON strings that are accepted by the JavaScriptSerializer class.
        /// The default value is 2097152.
        /// </summary>
        public int MaxJsonLength { get; set; }

        /// <summary>
        /// Gets or sets the limit for constraining the number of object levels to process.
        /// The default value is 100.
        /// </summary>
        public int RecursionLimit { get; set; }

        /// <summary>
        /// Initializes a new instance of the SerializationSettings class with the default settings.
        /// </summary>
        public SerializationSettings()
        {
            MaxJsonLength = 2097152;
            RecursionLimit = 100;
        }

        /// <summary>
        /// Initializes a new instance of the SerializationSettings class with the specified type resolver.
        /// </summary>
        public SerializationSettings(JavaScriptTypeResolver typeResolver)
            : this()
        {
            TypeResolver = typeResolver;
        }
    }
}
