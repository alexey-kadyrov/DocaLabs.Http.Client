using System;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_null_charset_and_the_response_has_content_type_with_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-16", new MemoryStream(Encoding.Unicode.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_null_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("text/plain", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_empty_charset_and_the_response_does_not_have_charset_in_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute
            {
                CharSet = ""
            };
            Setup("application/json", new MemoryStream(Encoding.GetEncoding(CharSets.Iso88591).GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_specified_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute
            {
                CharSet = CharSets.Utf16
            };
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.GetEncoding(CharSets.Utf16).GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_and_there_is_charset_on_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromJsonAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_return_null_object =
            () => target.ShouldBeNull();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static Exception exception;
        static DeserializeFromJsonAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    public class when_json_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromJsonAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromJsonAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_on_bad_json_value : response_deserialization_test_context
    {
        const string data = "} : Non JSON string : {";
        static DeserializeFromJsonAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "deserialization")]
    class when_json_deserializer_is_used_with_bad_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute
            {
                CharSet = "-bad-charset-"
            };
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static Exception exception;
        static DeserializeFromJsonAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    public class when_json_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromJsonAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromJsonAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_all_in_capital : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("APPLICATION/JSON", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_json_content_type_but_for_simple_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_non_json_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromJsonAttribute), "checking that can deserialize")]
    class when_json_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "{Value1:2012, Value2:\"Hello World!\"}";
        static DeserializeFromJsonAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromJsonAttribute();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }
}
