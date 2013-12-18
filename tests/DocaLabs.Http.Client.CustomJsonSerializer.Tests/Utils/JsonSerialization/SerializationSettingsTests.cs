using System;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    [TestClass]
    public class when_instantiating_serialization_settings_using_non_default_constructor
    {
        static SerializationSettings _settings;
        static JsonSerializerSettings _jsonSerializerSettings;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _jsonSerializerSettings = new JsonSerializerSettings();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _settings = new SerializationSettings(typeof (DateTime), Formatting.Indented, _jsonSerializerSettings);
        }

        [TestMethod]
        public void it_should_set_type_property_to_the_specified_value()
        {
            _settings.Type.ShouldBeTheSameAs(typeof (DateTime));
        }

        [TestMethod]
        public void it_should_set_formatting_property_to_the_specified_value()
        {
            _settings.Formatting.ShouldEqual(Formatting.Indented);
        }

        [TestMethod]
        public void it_should_set_settings_property_to_the_specified_value()
        {
            _settings.Settings.ShouldBeTheSameAs(_jsonSerializerSettings);
        }
    }
}
