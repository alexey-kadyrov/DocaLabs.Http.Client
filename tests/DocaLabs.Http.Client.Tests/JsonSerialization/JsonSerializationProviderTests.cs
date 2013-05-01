using System;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.JsonSerialization
{
    [Subject(typeof(JsonSerializationProvider))]
    class when_json_serialization_provider_is_used_in_default_configuration_without_any_extension_library_with_json_serialization
    {
        It should_return_default_json_serializer =
            () => JsonSerializationProvider.Serializer.ShouldBeOfType<DefaultJsonSerializer>();

        It should_return_default_json_deserializer =
            () => JsonSerializationProvider.Deserializer.ShouldBeOfType<DefaultJsonDeserializer>();
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_changing_serializer_in_json_serialization_provider
    {
        static IJsonSerializer original_serializer;
        static IJsonSerializer new_serializer;

        Cleanup after_each =
            () => JsonSerializationProvider.Serializer = original_serializer;

        Establish context = () =>
        {
            original_serializer = JsonSerializationProvider.Serializer;
            new_serializer = new TestSerializer();
        };

        Because of =
            () => JsonSerializationProvider.Serializer = new_serializer;

        It should_return_new_serializer =
            () => JsonSerializationProvider.Serializer.ShouldBeTheSameAs(new_serializer);

        class TestSerializer : IJsonSerializer
        {
            public string Serialize(object obj)
            {
                return "";
            }
        }
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_changing_deserializer_in_json_serialization_provider
    {
        static IJsonDeserializer original_deserializer;
        static IJsonDeserializer new_deserializer;

        Cleanup after_each =
            () => JsonSerializationProvider.Deserializer = original_deserializer;

        Establish context = () =>
        {
            original_deserializer = JsonSerializationProvider.Deserializer;
            new_deserializer = new TestDeserializer();
        };

        Because of =
            () => JsonSerializationProvider.Deserializer = new_deserializer;

        It should_return_new_serializer =
            () => JsonSerializationProvider.Deserializer.ShouldBeTheSameAs(new_deserializer);

        class TestDeserializer : IJsonDeserializer
        {
            public object Deserialize(string value, Type type)
            {
                return null;
            }
        }
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_reloading_serialization_extensions_without_any_extension_library_with_json_serialization
    {
        Establish context = () =>
        {
            JsonSerializationProvider.Serializer = new TestSerializer();
            JsonSerializationProvider.Deserializer = new TestDeserializer();
        };

        Because of =
            () => JsonSerializationProvider.ReloadSerializationExtensions();

        It should_restore_default_json_serializer =
            () => JsonSerializationProvider.Serializer.ShouldBeOfType<DefaultJsonSerializer>();

        It should_restore_default_json_deserializer =
            () => JsonSerializationProvider.Deserializer.ShouldBeOfType<DefaultJsonDeserializer>();

        class TestSerializer : IJsonSerializer
        {
            public string Serialize(object obj)
            {
                return "";
            }
        }

        class TestDeserializer : IJsonDeserializer
        {
            public object Deserialize(string value, Type type)
            {
                return null;
            }
        }
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_setting_serializer_to_null_in_json_serialization_provider
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => JsonSerializationProvider.Serializer = null);

        It should_return_previously_set_serializer =
            () => JsonSerializationProvider.Serializer.ShouldBeOfType<DefaultJsonSerializer>();

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_setting_deserializer_to_null_in_json_serialization_provider
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => JsonSerializationProvider.Deserializer = null);

        It should_return_previously_set_serializer =
            () => JsonSerializationProvider.Deserializer.ShouldBeOfType<DefaultJsonDeserializer>();

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(JsonSerializationProvider))]
    class when_json_serialization_provider_is_used_concurrently
    {
        static int count;
        static object locker;
        static Random random;

        Establish context = () =>
        {
            count = 0;
            locker = new object();
            random = new Random();
        };

        Because of = () => Parallel.For(0, 50000, i =>
        {
            int ii;

            lock (locker)
            {
                ii = random.Next(5);
            }

            switch (ii)
            {
                case 0:
                    JsonSerializationProvider.Serializer = new DefaultJsonSerializer();
                    break;
                case 1:
                    JsonSerializationProvider.Deserializer = new DefaultJsonDeserializer();
                    break;
                case 2:
                    JsonSerializationProvider.Serializer.ShouldBeOfType<DefaultJsonSerializer>();
                    break;
                case 3:
                    JsonSerializationProvider.Deserializer.ShouldBeOfType<DefaultJsonDeserializer>();
                    break;
                case 4:
                    JsonSerializationProvider.ReloadSerializationExtensions();
                    break;
            }

            Interlocked.Increment(ref count);
        });

        It should_execute_all_operations =
            () => count.ShouldEqual(50000);
    }
}
