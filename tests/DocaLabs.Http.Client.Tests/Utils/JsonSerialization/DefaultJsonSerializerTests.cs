using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using Moq;
using Newtonsoft.Json;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    [Subject(typeof(DefaultJsonSerializer))]
    class when_default_json_serializer_is_used
    {
        static TestTarget original_object;
        static string json_string;
        static DefaultJsonSerializer serializer;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            serializer = new DefaultJsonSerializer();
        };

        Because of =
            () => json_string = serializer.Serialize(original_object);

        It should_serialize_object =
            () => JsonConvert.DeserializeObject<TestTarget>(json_string).ShouldBeSimilar(original_object);
    }

    [Subject(typeof(DefaultJsonSerializer))]
    class when_updating_serialization_settings_with_null_type_for_default_json_serializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => DefaultJsonSerializer.UpdateSettings(null, new SerializationSettings()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(DefaultJsonSerializer))]
    class when_updating_serialization_settings_with_null_settings_for_default_json_serializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => DefaultJsonSerializer.UpdateSettings(typeof(TestTarget), null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_settings_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("settings");
    }

    [Subject(typeof(DefaultJsonSerializer))]
    class when_default_json_serializer_is_used_with_customized_type_resolver
    {
        static Model model;
        static Mock<JavaScriptTypeResolver> type_resolver;
        static string json_string;
        static DefaultJsonSerializer serializer;

        Establish context = () =>
        {
            type_resolver = new Mock<JavaScriptTypeResolver>() { CallBase = true };
            type_resolver.Setup(x => x.ResolveType(Moq.It.IsAny<string>())).Returns((string id) => Type.GetType(id));
            type_resolver.Setup(x => x.ResolveTypeId(Moq.It.IsAny<Type>())).Returns((Type type) => type.Name);

            DefaultJsonSerializer.UpdateSettings(typeof(Model), new SerializationSettings(type_resolver.Object));

            model = new Model
            {
                Value = "Hello World!"
            };

            serializer = new DefaultJsonSerializer();
        };

        Because of =
            () => json_string = serializer.Serialize(model);

        It should_serialize_object =
            () => JsonConvert.DeserializeObject<Model>(json_string).ShouldBeSimilar(model);

        It should_use_provided_type_resolver =
            () => type_resolver.Verify(x => x.ResolveTypeId(typeof (Model)));

        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultJsonSerializer))]
    class when_default_json_serializer_is_used_concurrently
    {
        static TestTarget original_object;
        static DefaultJsonSerializer serializer;
        static int count;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            serializer = new DefaultJsonSerializer();
        };

        Because of = () => Parallel.For(0, 500000, i =>
        {
            var target = serializer.Serialize(original_object);

            JsonConvert.DeserializeObject<TestTarget>(target).ShouldBeSimilar(original_object);

            Interlocked.Increment(ref count);
        });

        It should_deserialize_all_object =
            () => count.ShouldEqual(500000);
    }
}
