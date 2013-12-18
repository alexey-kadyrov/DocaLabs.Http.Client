using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    [TestClass]
    public class when_json_serializer_factory_is_asked_to_create_serializer
    {
        static JsonSerializerFactory _factory;
        static IJsonSerializer _serializer;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new JsonSerializerFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _serializer = _factory.CreateSerializer();
        }

        [TestMethod]
        public void it_should_return_instance_of_json_serializer_class()
        {
            _serializer.ShouldBeOfType<JsonSerializer>();
        }
    }

    [TestClass]
    public class when_json_serializer_factory_is_asked_to_create_deserializer
    {
        static JsonSerializerFactory _factory;
        static IJsonDeserializer _deserializer;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new JsonSerializerFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _deserializer = _factory.CreateDeserializer();
        }

        [TestMethod]
        public void it_should_return_instance_of_json_deserializer_class()
        {
            _deserializer.ShouldBeOfType<JsonDeserializer>();
        }
    }
}
