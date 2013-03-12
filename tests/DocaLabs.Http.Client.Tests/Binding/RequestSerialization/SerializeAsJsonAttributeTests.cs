using System;
using System.Text;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.ContentEncoding;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.RequestSerialization
{
    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsJsonAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsJsonAttribute();
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsJson<TestTarget>().ShouldBeSimilar(original_object);
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_with_gzip_content_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsJsonAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsJsonAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object =
            () => ParseDecodedRequestDataAsJson<TestTarget>().ShouldBeSimilar(original_object);
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_with_utf_32_charset : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsJsonAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsJsonAttribute { CharSet = Encoding.UTF32.WebName };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-32");

        It should_serialize_object =
            () => ParseRequestDataAsJson<TestTarget>(Encoding.UTF32).ShouldBeSimilar(original_object);
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    public class when_serialize_as_json_attribute_is_newed
    {
        static SerializeAsJsonAttribute attribute;

        Because of =
            () => attribute = new SerializeAsJsonAttribute();

        It should_set_charset_to_utf8 =
            () => attribute.CharSet.ShouldEqual(Encoding.UTF8.WebName);

        It should_set_request_content_encoding_to_null =
            () => attribute.RequestContentEncoding.ShouldBeNull();
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_with_null_object : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => attribute.Serialize(null, mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_serialize_to_empty_string =
            () => GetRequestData().ShouldBeEmpty();
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    public class when_serialize_as_json_attribute_is_used_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsJsonAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsJsonAttribute { CharSet = Encoding.UTF32.WebName };
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }
}
