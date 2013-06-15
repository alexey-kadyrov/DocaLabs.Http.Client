using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using Newtonsoft.Json;

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
