using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(SerializeStreamAttribute))]
    public class when_serialize_stream_attribute_is_newed
    {
        static SerializeStreamAttribute attribute;

        Because of =
            () => attribute = new SerializeStreamAttribute();

        It should_set_request_content_encoding_to_null =
            () => attribute.RequestContentEncoding.ShouldBeNull();

        It should_set_request_content_type_to_application_octet =
            () => attribute.ContentType.ShouldEqual("application/octet-stream");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_serialize_stream_attribute_is_used_with_null_object : request_serialization_test_context
    {
        static SerializeStreamAttribute attribute;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => attribute.Serialize(null, mock_web_request.Object);

        It should_set_request_content_type_as_application_octet =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_to_empty_stream =
            () => GetRequestDataLength().ShouldEqual(0);
    }

    [Subject(typeof(SerializeStreamAttribute))]
    public class when_serialize_stream_attribute_is_used_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static SerializeStreamAttribute attribute;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_serialize_stream_attribute_is_used_with_non_stream_object : request_serialization_test_context
    {
        static SerializeStreamAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize("Hello World!", mock_web_request.Object));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_request_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("obj");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_setting_content_type_in_serialize_stream_attribute_to_null
    {
        static SerializeStreamAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.ContentType = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_setting_content_type_in_serialize_stream_attribute_to_empty_string
    {
        static SerializeStreamAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.ContentType = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_serialize_stream_attribute_is_used_for_stream : request_serialization_test_context
    {
        static SerializeStreamAttribute attribute;

        Establish context =
            () => attribute = new SerializeStreamAttribute();

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_application_octet =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_serialize_stream_attribute_is_used_for_stream_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeStreamAttribute attribute;

        Establish context =
            () => attribute = new SerializeStreamAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_application_octet =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => GetDecodedRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeStreamAttribute))]
    class when_serialize_stream_attribute_is_used_for_stream_with_custom_content_type : request_serialization_test_context
    {
        static SerializeStreamAttribute attribute;

        Establish context =
            () => attribute = new SerializeStreamAttribute { ContentType = "some-custom-type" };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_specified_content_type =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("some-custom-type");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }
}
