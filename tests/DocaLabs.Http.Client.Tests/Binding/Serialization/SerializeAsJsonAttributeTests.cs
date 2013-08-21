﻿using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
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

        It should_set_request_content_type_as_application_json_with_utf_32_charset =
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
            () => attribute.CharSet.ShouldEqual(CharSets.Utf8);

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

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_setting_charset_in_serialize_as_json_attribute_to_null
    {
        static SerializeAsJsonAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_setting_charset_in_serialize_as_json_attribute_to_empty_string
    {
        static SerializeAsJsonAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.CharSet = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_setting_charset_in_serialize_as_json_attribute_to_unknown_charset : request_serialization_test_context
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

            attribute = new SerializeAsJsonAttribute { CharSet = "-unknown-charset-" };
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }
    
    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_string : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context = 
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => attribute.Serialize("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}", mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_string_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}", mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => ParseDecodedRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_string_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}", mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-32");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>(Encoding.UTF32).ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_byte_array : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => attribute.Serialize(Encoding.UTF8.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}"), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_byte_array_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(Encoding.UTF8.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}"), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => ParseDecodedRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_byte_array_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize(Encoding.UTF32.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}"), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-32");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>(Encoding.UTF32).ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_stream : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute();

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}")), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_stream_with_gzip_content_encoding : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF8.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}")), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_8_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object = () => ParseDecodedRequestDataAsJson<TestTarget>().ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(SerializeAsJsonAttribute))]
    class when_serialize_as_json_attribute_is_used_on_already_serialized_stream_with_utf_32_charset : request_serialization_test_context
    {
        static SerializeAsJsonAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsJsonAttribute { CharSet = Encoding.UTF32.WebName };

        Because of =
            () => attribute.Serialize(new MemoryStream(Encoding.UTF32.GetBytes("{\"Value1\" : 2012, \"Value2\" : \"Hello World!\"}")), mock_web_request.Object);

        It should_set_request_content_type_as_application_json_with_utf_32_charset =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/json; charset=utf-32");

        It should_serialize_object = () => ParseRequestDataAsJson<TestTarget>(Encoding.UTF32).ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }
}
