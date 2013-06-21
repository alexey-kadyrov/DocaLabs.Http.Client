using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.ResponseDeserialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.ResponseDeserialization
{
    [Subject(typeof(DeserializeFromJsonAttribute))]
    class when_deserialize_from_json_attribute_is_used : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute))]
    class when_deserialize_from_json_attribute_is_used_and_content_type_does_not_have_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromJsonAttribute();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute))]
    class when_deserialize_from_json_attribute_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromJsonAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response_stream, typeof(TestTarget));

        It should_return_null_object =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(DeserializeFromJsonAttribute))]
    class when_deserialize_from_json_attribute_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static Exception exception;
        static DeserializeFromJsonAttribute attribute;

        Establish context = () =>
        {
            attribute = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromJsonAttribute))]
    public class when_deserialize_from_json_attribute_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromJsonAttribute attribute;

        Establish context =
            () => attribute = new DeserializeFromJsonAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(DeserializeFromJsonAttribute))]
    class when_deserialize_from_json_attribute_is_used_on_bad_json_value : response_deserialization_test_context
    {
        const string data = "} : Non JSON string : {";
        static DeserializeFromJsonAttribute attribute;
        static Exception exception;

        Establish context = () =>
        {
            attribute = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }
}
