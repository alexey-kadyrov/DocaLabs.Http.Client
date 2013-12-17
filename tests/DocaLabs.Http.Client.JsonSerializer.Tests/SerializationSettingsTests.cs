using System;
using Machine.Specifications;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft.Tests
{
    [Subject(typeof(SerializationSettings))]
    class when_instantiating_serialization_settings_using_non_default_constructor
    {
        static SerializationSettings settings;
        static JsonSerializerSettings json_serializer_settings;

        Establish context =
            () => json_serializer_settings = new JsonSerializerSettings();

        Because of =
            () => settings = new SerializationSettings(typeof (DateTime), Formatting.Indented, json_serializer_settings);

        It should_set_type_property_to_the_specified_value =
            () => settings.Type.ShouldBeTheSameAs(typeof (DateTime));

        It should_set_formatting_property_to_the_specified_value =
            () => settings.Formatting.ShouldEqual(Formatting.Indented);

        It should_set_settings_property_to_the_specified_value =
            () => settings.Settings.ShouldBeTheSameAs(json_serializer_settings);
    }
}
