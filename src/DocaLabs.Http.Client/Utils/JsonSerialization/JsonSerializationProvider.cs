using System;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    /// <summary>
    /// Provides implementations for serialization objects in JSON notation. All public methods and properties are thread safe.
    /// When the type is accessed for the first time it scans the base folder using MEF for exports of IJsonSerializer
    /// and IJsonDeserializer in assemblies with pattern "DocaLabs.Http.Client.Extension.*" if there is nothing found then it will use
    /// DefaultJsonSerializer and DefaultJsonDeserializer.
    /// All members are thread safe.
    /// </summary>
    public static class JsonSerializationProvider
    {
        static IJsonSerializer _serializer;
        static IJsonDeserializer _deserializer;

        /// <summary>
        /// Gets or sets the json serializer implementation. The property cannot be set to null.
        /// </summary>
        public static IJsonSerializer Serializer
        {
            get
            {
                return _serializer;
            }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _serializer = value;
            }
        }

        /// <summary>
        /// Gets or sets the json deserializer implementation. The property cannot be set to null.
        /// </summary>
        public static IJsonDeserializer Deserializer
        {
            get
            {
                return _deserializer;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _deserializer = value;
            }
        }

        static JsonSerializationProvider()
        {
            var serializerFactory = PlatformAdapter.Resolve<IJsonSerializerFactory>(false, "DocaLabs.Http.Client.JsonSerializer");

            if (serializerFactory != null)
            {
                _serializer = serializerFactory.CreateSerializer();
                _deserializer = serializerFactory.CreateDeserializer();
            }

            if (_serializer == null)
                _serializer = new DefaultJsonSerializer();

            if (_deserializer == null)
                _deserializer = new DefaultJsonDeserializer();
        }
    }
}
