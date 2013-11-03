using System.ComponentModel.Composition;

namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    public class JasonSerializerFactory : IJasonSerializerFactory
    {
        readonly IJsonSerializer _serializer;
        readonly IJsonDeserializer _deserializer;

        public JasonSerializerFactory()
        {
            var loader = new ExtensionLoader();

            using (var composer = new LibraryExtensionsComposer())
            {
                composer.ComposePartsFor(loader);
            }

            _serializer = loader.SerializerExtension;
            _deserializer = loader.DeserializerExtension;
        }

        public IJsonSerializer CreateSerializer()
        {
            return _serializer;
        }

        public IJsonDeserializer CreateDeserializer()
        {
            return _deserializer;
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public IJsonSerializer SerializerExtension { get; set; }

            [Import(AllowDefault = true)]
            public IJsonDeserializer DeserializerExtension { get; set; }
        }
    }
}
