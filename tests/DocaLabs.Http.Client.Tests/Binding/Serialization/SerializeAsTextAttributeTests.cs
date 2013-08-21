using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(SerializeAsTextAttribute))]
    public class when_serialize_as_text_attribute_is_newed
    {
        static SerializeAsTextAttribute attribute;

        Because of =
            () => attribute = new SerializeAsTextAttribute();

        It should_set_charset_to_utf8 =
            () => attribute.CharSet.ShouldEqual(CharSets.Utf8);

        It should_set_request_content_encoding_to_null =
            () => attribute.RequestContentEncoding.ShouldBeNull();

        It should_set_request_content_type_to_plain_text =
            () => attribute.ContentType.ShouldEqual("text/plain");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_with_null_object : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => attribute.Serialize(null, mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_serialize_to_empty_string =
            () => GetRequestData().ShouldBeEmpty();
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    public class when_serialize_as_text_attribute_is_used_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static SerializeAsTextAttribute attribute;

        Establish context = 
            () => attribute = new SerializeAsTextAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize("Hello World!", null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_setting_charset_in_serialize_as_text_attribute_to_null
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_setting_charset_in_serialize_as_text_attribute_to_empty_string
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_setting_content_type_in_serialize_as_text_attribute_to_null
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.ContentType = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_setting_content_type_in_serialize_as_text_attribute_to_empty_string
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.ContentType = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_setting_charset_in_serialize_as_text_attribute_to_unknown_charset : request_serialization_test_context
    {
        static Exception exception;
        static SerializeAsTextAttribute attribute;

        Establish context = 
            () => attribute = new SerializeAsTextAttribute { CharSet = "-unknown-charset-" };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize("Hello World!", mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }
    
    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_string : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context = 
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => attribute.Serialize("Hello World!", mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_string_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize("Hello World!", mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => GetDecodedRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_string_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize("Hello World!", mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-32");

        It should_serialize_object = () => GetRequestData(Encoding.UTF32).ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_string_for_custom_content_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { ContentType = "text/custom"};

        Because of =
            () => attribute.Serialize("Hello World!", mock_web_request.Object);

        It should_set_request_content_type_as_custom_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/custom; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_byte_array : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => attribute.Serialize(Encoding.UTF8.GetBytes("Hello World!"), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_byte_array_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(Encoding.UTF8.GetBytes("Hello World!"), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => GetDecodedRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_byte_array_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize(Encoding.UTF32.GetBytes("Hello World!"), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-32");

        It should_serialize_object = () => GetRequestData(Encoding.UTF32).ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_byte_array_for_custom_content_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { ContentType = "text/custom" };

        Because of =
            () => attribute.Serialize(Encoding.UTF8.GetBytes("Hello World!"), mock_web_request.Object);

        It should_set_request_content_type_as_custom_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/custom; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_stream : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_stream_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => GetDecodedRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_stream_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-32");

        It should_serialize_object = () => GetRequestData(Encoding.UTF32).ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_stream_for_custom_content_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { ContentType = "text/custom" };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")), mock_web_request.Object);

        It should_set_request_content_type_as_custom_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/custom; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("Hello World!");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_simple_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => attribute.Serialize(42, mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("42");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_simple_type_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(42, mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => GetDecodedRequestData().ShouldEqual("42");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_simple_type_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize(42, mock_web_request.Object);

        It should_set_request_content_type_as_plain_text_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/plain; charset=utf-32");

        It should_serialize_object = () => GetRequestData(Encoding.UTF32).ShouldEqual("42");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_simple_type_for_custom_content_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { ContentType = "text/custom" };

        Because of =
            () => attribute.Serialize(42, mock_web_request.Object);

        It should_set_request_content_type_as_custom_text_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/custom; charset=utf-8");

        It should_serialize_object = () => GetRequestData().ShouldEqual("42");
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_non_simple_type : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(new object(), mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();
    }

    [Subject(typeof(SerializeAsTextAttribute))]
    class when_serialize_as_text_attribute_is_used_for_non_simple_type_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsTextAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsTextAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(new object(), mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();
    }
}
