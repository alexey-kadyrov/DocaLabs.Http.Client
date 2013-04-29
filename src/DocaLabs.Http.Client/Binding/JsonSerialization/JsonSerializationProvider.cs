using System;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Binding.Utils;

namespace DocaLabs.Http.Client.Binding.JsonSerialization
{
    /// <summary>
    /// Provides implementations for serialization objects in JSON notation. All public methods and properties are thread safe.
    /// When the type is accessed for the first time it scans the base folder using MEF for exports of IJsonSerializer
    /// and IJsonDeserializer in assemblies with prefix "DocaLabs.Extensions." if there is nothing found then it will use
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

            ReloadSerializationExtensions();
        }

        /// <summary>
        /// Scans the base folder using MEF for exports of IJsonSerializer and IJsonDeserializer in assemblies with 
        /// prefix "DocaLabs.Extensions." if there is nothing found then it will use DefaultJsonSerializer and DefaultJsonDeserializer.
        /// Normally there is no need to call the method, call it if you want to force the scan early as it can be quite
        /// expensive operation if done during first serialization/deserialization.
        /// </summary>
        public static void ReloadSerializationExtensions()
        {
            var loader = new ExtensionLoader();

            using (var composer = new LibraryExtensionsComposer())
            {
                composer.ComposePartsFor(loader);
            }

            lock (Locker)
            {
                _serializer = loader.SerializerExtension ?? new DefaultJsonSerializer();
                _deserializer = loader.DeserializerExtension ?? new DefaultJsonDeserializer();
            }
        }

        class ExtensionLoader
        {
            [Import]
            public IJsonSerializer SerializerExtension { get; set; }

            [Import]
            public IJsonDeserializer DeserializerExtension { get; set; }
        }
    }
}
