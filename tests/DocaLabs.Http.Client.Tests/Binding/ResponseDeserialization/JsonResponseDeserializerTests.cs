using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.ResponseDeserialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.ResponseDeserialization
{
    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    class when_json_deserializer_is_used : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    class when_json_deserializer_is_used_and_there_is_charset_on_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    class when_json_deserializer_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static JsonResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_return_null_object =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    class when_json_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static Exception exception;
        static JsonResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    public class when_json_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static JsonResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new JsonResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(JsonResponseDeserializer), "deserialization")]
    class when_json_deserializer_is_used_on_bad_json_value : response_deserialization_test_context
    {
        const string data = "} : Non JSON string : {";
        static JsonResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static Exception exception;
        static JsonResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    public class when_json_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static JsonResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new JsonResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }


    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_all_in_capital : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("APPLICATION/JSON", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_but_for_simple_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_non_json_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(JsonResponseDeserializer), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static JsonResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new JsonResponseDeserializer();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }
}
