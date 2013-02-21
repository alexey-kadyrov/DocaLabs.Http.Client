using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.ResponseDeserialization;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.ResponseDeserialization
{
    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_for_string_result : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static PlainTextResponseDeserializer deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response, typeof(string));

        It should_deserialize_string = 
            () => target.ShouldEqual("Hello World!");
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_for_decimal_result : response_deserialization_test_context
    {
        const string data = "42.55";
        static PlainTextResponseDeserializer deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.Deserialize(http_response, typeof(decimal));

        It should_deserialize_decimal =
            () => target.ShouldEqual(42.55M);
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_with_empty_response_stream_for_string_result : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;
        static string target;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (string)deserializer.Deserialize(http_response, typeof(string));

        It should_return_null =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_with_empty_response_stream_for_decimal_result : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;
        static decimal target;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (decimal)deserializer.Deserialize(http_response, typeof(decimal));

        It should_return_default_value_for_decimal =
            () => target.ShouldEqual(default(decimal));
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "Hello World!";
        static Exception exception;
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    public class when_plain_text_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static PlainTextResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new PlainTextResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(string)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(PlainTextResponseDeserializer), "deserialization")]
    class when_plain_text_deserializer_is_used_on_string_that_cannot_be_cnverted_to_target_type : response_deserialization_test_context
    {
        const string data = "} : non numerical string : {";
        static PlainTextResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(int)));

        It should_throw_unrecoverable_http_client_exception =
            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "12";
        static Exception exception;
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    public class when_plain_text_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static PlainTextResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new PlainTextResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(decimal)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static PlainTextResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_none_plain_text_or_html_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static PlainTextResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_plain_text_content_type : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response, typeof(string)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response, typeof(int)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response, typeof(long)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response, typeof(double)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response, typeof(decimal)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response, typeof(Guid)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response, typeof(DateTime)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response, typeof(DateTimeOffset)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response, typeof(TimeSpan)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response, typeof(TestEnum)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response, typeof(bool)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response, typeof(char)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_plain_text_deserializer_is_checking_response_with_plain_text_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/plain", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response, typeof(string)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response, typeof(int)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response, typeof(long)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response, typeof(double)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response, typeof(decimal)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response, typeof(Guid)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response, typeof(DateTime)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response, typeof(DateTimeOffset)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response, typeof(TimeSpan)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response, typeof(TestEnum)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response, typeof(bool)).ShouldBeTrue();

        It should_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response, typeof(char)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_html_text_deserializer_is_checking_response_with_plain_text_content_type : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/html; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum 
        {
        }
    }

    [Subject(typeof(PlainTextResponseDeserializer), "checking that can deserialize")]
    class when_html_text_deserializer_is_checking_response_with_plain_text_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "";
        static PlainTextResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new PlainTextResponseDeserializer();
            Setup("text/html", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        It should_be_able_to_deserialize_for_string =
            () => deserializer.CanDeserialize(http_response, typeof(string)).ShouldBeTrue();

        It should_not_be_able_to_deserialize_for_int =
            () => deserializer.CanDeserialize(http_response, typeof(int)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_long =
            () => deserializer.CanDeserialize(http_response, typeof(long)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_double =
            () => deserializer.CanDeserialize(http_response, typeof(double)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_decimal =
            () => deserializer.CanDeserialize(http_response, typeof(decimal)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_guid =
            () => deserializer.CanDeserialize(http_response, typeof(Guid)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetime =
            () => deserializer.CanDeserialize(http_response, typeof(DateTime)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_datetimeoffset =
            () => deserializer.CanDeserialize(http_response, typeof(DateTimeOffset)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_timespan =
            () => deserializer.CanDeserialize(http_response, typeof(TimeSpan)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_enum =
            () => deserializer.CanDeserialize(http_response, typeof(TestEnum)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_bool =
            () => deserializer.CanDeserialize(http_response, typeof(bool)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_char =
            () => deserializer.CanDeserialize(http_response, typeof(char)).ShouldBeFalse();

        It should_not_be_able_to_deserialize_for_refrence_type =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();

        enum TestEnum
        {
        }
    }
}
