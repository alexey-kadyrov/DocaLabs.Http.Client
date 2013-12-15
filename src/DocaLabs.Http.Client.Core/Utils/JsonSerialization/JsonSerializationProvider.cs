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
        static readonly object Locker;
        static IJsonSerializer _serializer;
        static IJsonDeserializer _deserializer;

        /// <summary>
        /// Gets or sets the json serializer implementation. The property cannot be set to null.
        /// </summary>
        public static IJsonSerializer Serializer
        {
            get
            {
                lock (Locker)
                {
                    return _serializer;
                }
            }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                lock (Locker)
                {
                    _serializer = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the json deserializer implementation. The property cannot be set to null.
        /// </summary>
        public static IJsonDeserializer Deserializer
        {
            get
            {
                lock (Locker)
                {
                    return _deserializer;
                }
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                lock (Locker)
                {
                    _deserializer = value;
                }
            }
        }

        static JsonSerializationProvider()
        {
            Locker = new object();

            var serializerFactory = PlatformAdapter.Resolve<IJasonSerializerFactory>(false);

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
