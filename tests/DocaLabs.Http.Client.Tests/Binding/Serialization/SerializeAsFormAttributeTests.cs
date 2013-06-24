using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(SerializeAsFormAttribute))]
    class when_serialize_as_form_attribute_is_used : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsFormAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsFormAttribute();
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_url_encoded_form_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldEqual("application/x-www-form-urlencoded; charset=utf-8");

        It should_serialize_all_properties =
            () => HttpUtility.ParseQueryString(GetRequestData()).ShouldContainOnly(
                new KeyValuePair<string, string>("Value1", "2012"),
                new KeyValuePair<string, string>("Value2", "Hello World!"));

        It should_properly_url_encode_values =
            () => GetRequestData().ShouldContain("Hello+World!");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_serialize_as_form_attribute_is_used_with_gzip_content_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsFormAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsFormAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_url_encoded_form_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldEqual("application/x-www-form-urlencoded; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_all_properties =
            () => HttpUtility.ParseQueryString(GetDecodedRequestData()).ShouldContainOnly(
                new KeyValuePair<string, string>("Value1", "2012"),
                new KeyValuePair<string, string>("Value2", "Hello World!"));

        It should_properly_url_encode_values =
            () => GetDecodedRequestData().ShouldContain("Hello+World!");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_serialize_as_form_attribute_is_used_with_utf_32_charset : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsFormAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsFormAttribute { CharSet = Encoding.UTF32.WebName };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_url_encoded_form_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldEqual("application/x-www-form-urlencoded; charset=utf-32");

        It should_serialize_all_properties =
            () => HttpUtility.ParseQueryString(GetRequestData(Encoding.UTF32)).ShouldContainOnly(
                new KeyValuePair<string, string>("Value1", "2012"),
                new KeyValuePair<string, string>("Value2", "Hello World!"));

        It should_properly_url_encode_values =
            () => GetRequestData(Encoding.UTF32).ShouldContain("Hello+World!");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    public class when_serialize_as_form_attribute_is_newed
    {
        static SerializeAsFormAttribute attribute;

        Because of =
            () => attribute = new SerializeAsFormAttribute();

        It should_set_charset_to_utf8 =
            () => attribute.CharSet.ShouldEqual(CharSets.Utf8);

        It should_set_request_content_encoding_to_null =
            () => attribute.RequestContentEncoding.ShouldBeNull();
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_serialize_as_form_attribute_is_used_with_null_object : request_serialization_test_context
    {
        static SerializeAsFormAttribute attribute;

        Establish context = 
            () => attribute = new SerializeAsFormAttribute();

        Because of =
            () => attribute.Serialize(null, mock_web_request.Object);

        It should_set_request_content_type_as_url_encoded_form_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldEqual("application/x-www-form-urlencoded; charset=utf-8");

        It should_serialize_to_empty_string =
            () => GetRequestData().ShouldBeEmpty();
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    public class when_serialize_as_form_attribute_is_used_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsFormAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsFormAttribute { CharSet = Encoding.UTF32.WebName };
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_setting_charset_in_serialize_as_form_attribute_to_null
    {
        static SerializeAsFormAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsFormAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_setting_charset_in_serialize_as_form_attribute_to_empty_string
    {
        static SerializeAsFormAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsFormAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsFormAttribute))]
    class when_setting_charset_in_serialize_as_form_attribute_to_unknown_charset : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsFormAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsFormAttribute { CharSet = "-unknown-charset-" };
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }
}
