using System;
using System.IO;
using System.Text;
using System.Threading;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_plain_text_with_null_charset_and_the_response_has_content_type_with_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string = 
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_plain_text_with_null_charset_and_the_response_has_content_type_with_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_plain_text_with_null_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_plain_text_with_null_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_plain_text_with_empty_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = ""
            };
            Setup("text/plain", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_plain_text_with_empty_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = ""
            };
            Setup("text/plain", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_plain_text_with_specified_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = CharSets.Utf16
            };
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.Unicode.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_plain_text_with_specified_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = CharSets.Utf16
            };
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.Unicode.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_plain_text_with_bad_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = "-bad-charset-"
            };
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.Unicode.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(string)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_plain_text_with_bad_charset : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute
            {
                CharSet = "-bad-charset-"
            };
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.Unicode.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Wait());

        It should_throw_http_client_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_html_text : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/html; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_html_text : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/html; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_text_xml : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_text_xml : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_application_xml : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_application_xml : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result_on_application_json : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_string_result_on_application_json : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_deserialize_string =
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_for_decimal_result : response_deserialization_test_context
    {
        const string data = "42.55";
        static DeserializeFromPlainTextAttribute deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.Deserialize(http_response_stream, typeof(decimal));

        It should_deserialize_decimal =
            () => target.ShouldEqual(42.55M);
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_for_decimal_result : response_deserialization_test_context
    {
        const string data = "42.55";
        static DeserializeFromPlainTextAttribute deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.DeserializeAsync(http_response_stream, typeof(decimal), CancellationToken.None).Result;

        It should_deserialize_decimal =
            () => target.ShouldEqual(42.55M);
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_with_empty_response_stream_for_string_result : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response_stream, typeof(string));

        It should_return_null =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_with_empty_response_stream_for_string_result : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.DeserializeAsync(http_response_stream, typeof(string), CancellationToken.None).Result;

        It should_return_null =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_with_empty_response_stream_for_decimal_result : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.Deserialize(http_response_stream, typeof(decimal));

        It should_return_default_value_for_decimal =
            () => target.ShouldEqual(default(decimal));
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_with_empty_response_stream_for_decimal_result : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.DeserializeAsync(http_response_stream, typeof(decimal), CancellationToken.None).Result;

        It should_return_default_value_for_decimal =
            () => target.ShouldEqual(default(decimal));
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_with_null_result_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, null, CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromPlainTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(string)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromPlainTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(null, typeof(string), CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "deserialization")]
    class when_plain_text_deserializer_is_used_on_string_that_cannot_be_cnverted_to_target_type : response_deserialization_test_context
    {
        const string data = "} : non numerical string : {";
        static DeserializeFromPlainTextAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(int)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "async deserialization")]
    class when_plain_text_deserializer_is_used_asynchronously_on_string_that_cannot_be_cnverted_to_target_type : response_deserialization_test_context
    {
        const string data = "} : non numerical string : {";
        static DeserializeFromPlainTextAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(int), CancellationToken.None).Wait());

        It should_throw_http_client_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "12";
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromPlainTextAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(decimal)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromPlainTextAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_unsupported_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromPlainTextAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("image/gif", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_plain_text_content_type : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_plain_text_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/plain", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_text_html_content_type : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/html; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum 
        {
        }
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_text_xml_content_type : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_application_xml_content_type : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_textdeserializer_is_checking_response_with_application_json_content_type : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }
   
    [Subject(typeof(DeserializeFromPlainTextAttribute), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_html_text_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromPlainTextAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromPlainTextAttribute();
            Setup("text/html", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response_stream, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response_stream, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response_stream, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response_stream, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response_stream, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response_stream, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response_stream, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response_stream, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response_stream, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response_stream, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }
}
