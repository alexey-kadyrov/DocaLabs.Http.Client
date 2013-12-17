using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Moq;
using Newtonsoft.Json;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Extension.NewtonSoft.Tests
{
    [Subject(typeof(JsonSerializer))]
    class when_json_serializer_is_used
    {
        static Model model;
        static string result;
        static JsonSerializer serializer;

        Establish context = () =>
        {
            model = new Model
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            serializer = new JsonSerializer();
        };

        Because of =
            () => result = serializer.Serialize(model);

        It should_serialize_object =
            () => JsonConvert.DeserializeObject<Model>(result).ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    [Subject(typeof(JsonSerializer))]
    class when_updating_serialization_settings_with_null_type_for_json_serializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => JsonSerializer.UpdateSettings(null, new SerializationSettings()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(JsonSerializer))]
    class when_updating_serialization_settings_with_null_settings_for_json_serializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => JsonSerializer.UpdateSettings(typeof(Model), null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_settings_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("settings");

        class Model
        {
            [UsedImplicitly]
            public int Value1 { get; set; }

            [UsedImplicitly]
            public string Value2 { get; set; }
        }
    }

    [Subject(typeof(JsonSerializer))]
    class when_json_serializer_is_used_with_customized_json_serializer_settings
    {
        static Model model;
        static Mock<SerializationBinder> binder;
        static string json_string;
        static JsonSerializer serializer;
        static string binded_assembly = "";
        static string binded_name = "Model";

        Establish context = () =>
        {
            binder = new Mock<SerializationBinder> { CallBase = true };
            binder.Setup(x => x.BindToName(typeof(Model), out binded_assembly, out binded_name));

            JsonSerializer.UpdateSettings(typeof(Model), new SerializationSettings { Settings = new JsonSerializerSettings
            {
                Binder = binder.Object,
                TypeNameHandling = TypeNameHandling.All
            }});

            model = new Model
            {
                Value = "Hello World!"
            };

            serializer = new JsonSerializer();
        };

        Because of =
            () => json_string = serializer.Serialize(model);

        It should_serialize_object =
            () => JsonConvert.DeserializeObject<Model>(json_string).Value.ShouldEqual("Hello World!");

        It should_use_provided_settings =
            () => binder.Verify(x => x.BindToName(typeof(Model), out binded_assembly, out binded_name));

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(JsonSerializer))]
    class when_json_serializer_is_used_with_customized_type
    {
        static Model model;
        static Mock<SerializationBinder> binder;
        static string json_string;
        static JsonSerializer serializer;
        static string binded_assembly = "";
        static string binded_name = "Model";

        Establish context = () =>
        {
            binder = new Mock<SerializationBinder> { CallBase = true };
            binder.Setup(x => x.BindToName(typeof(Model), out binded_assembly, out binded_name));

            JsonSerializer.UpdateSettings(typeof(Model), new SerializationSettings
            {
                Settings = new JsonSerializerSettings
                {
                    Binder = binder.Object,
                    TypeNameHandling = TypeNameHandling.Auto
                },
                Type = typeof(object)
            });

            model = new Model
            {
                Value = "Hello World!"
            };

            serializer = new JsonSerializer();
        };

        Because of =
            () => json_string = serializer.Serialize(model);

        It should_serialize_object =
            () => JsonConvert.DeserializeObject<Model>(json_string).Value.ShouldEqual("Hello World!");

        It should_use_provided_type =
            () => binder.Verify(x => x.BindToName(typeof(Model), out binded_assembly, out binded_name));

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(JsonSerializer))]
    class when_json_serializer_is_used_concurrently
    {
        static Model original_object;
        static JsonSerializer serializer;
        static int count;

        Establish context = () =>
        {
            original_object = new Model
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            serializer = new JsonSerializer();
        };

        Because of = () => Parallel.For(0, 10000, i =>
        {
            var target = serializer.Serialize(original_object);

            JsonConvert.DeserializeObject<Model>(target).ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

            Interlocked.Increment(ref count);
        });

        It should_deserialize_all_object =
            () => count.ShouldEqual(10000);

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }
}
