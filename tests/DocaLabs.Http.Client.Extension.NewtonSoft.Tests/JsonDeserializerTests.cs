using System;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft.Tests
{
    [Subject(typeof(JsonDeserializer))]
    class when_json_deserializer_is_used
    {
        static JsonDeserializer deserializer;
        static Model model;

        Establish context =
            () => deserializer = new JsonDeserializer();

        Because of =
            () => model = (Model)deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof(Model));

        It should_deserialize_object =
            () => model.ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

        class Model
        {
            public int Value1 { get; [UsedImplicitly] set; }
            public string Value2 { get; [UsedImplicitly] set; }
        }
    }

    [Subject(typeof(JsonDeserializer))]
    class when_json_deserializer_is_used_with_null_result_type
    {
        static JsonDeserializer deserializer;
        static Exception exception;

        Establish context =
            () => deserializer = new JsonDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(JsonDeserializer))]
    class when_updating_serialization_settings_with_null_type_for_json_deserializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => JsonDeserializer.UpdateSettings(null, new JsonSerializerSettings()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(JsonDeserializer))]
    class when_json_deserializer_is_used_with_json_serializer_settings
    {
        static JsonDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            JsonDeserializer.UpdateSettings(typeof(Model), new JsonSerializerSettings { MaxDepth = 1 });

            deserializer = new JsonDeserializer();
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize("{Value:\"Hello World!\", AnotherModel : { \"__type\" : \"InnerModel\", \"InnerValue\" : \"Value42\"}}", typeof(Model)));

        It should_use_that_settings =
            () => exception.ShouldNotBeNull();

        class Model
        {
            [UsedImplicitly]
            public string Value { get; set; }

            [UsedImplicitly]
            public object AnotherModel { get; [UsedImplicitly] set; }
        }

        class InnerModel
        {
            [UsedImplicitly]
            public string InnerValue { get; [UsedImplicitly] set; }
        }
    }

    [Subject(typeof(JsonDeserializer))]
    class when_json_deserializer_is_used_concurrently
    {
        static JsonDeserializer deserializer;
        static int count;

        Establish context =
            () => deserializer = new JsonDeserializer();

        Because of = () => Parallel.For(0, 10000, i =>
        {
            var model = (Model)deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof(Model));

            model.ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

            Interlocked.Increment(ref count);
        });

        It should_deserialize_all_object =
            () => count.ShouldEqual(10000);

        public class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }
}
