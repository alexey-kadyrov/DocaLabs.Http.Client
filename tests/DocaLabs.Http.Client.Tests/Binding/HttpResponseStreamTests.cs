using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    public class HttpResponseInBodyTestClass
    {
        public int Value1 { get; set; }
        public string Value2 { get; set; }
    }

    [Subject(typeof(HttpResponseStream))]
    class when_http_response_is_initialized_but_not_used_and_then_disposed_it_releases_all_resources : response_deserialization_test_context
    {
        static Stream response_stream;

        Establish context = () =>
        {
            response_stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            Setup("text/plain; charset=utf-8", response_stream);
        };

        Because of =
            () => http_response_stream.Dispose();

        It should_close_the_underlying_web_response =
            () => mock_response.Verify(x => x.Close(), Times.AtLeastOnce());

        It should_dispose_the_response_stream =
            () => (Catch.Exception(() => response_stream.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
    }

    [Subject(typeof(HttpResponseStream))]
    class when_http_response_is_initialized_and_used_and_then_disposed_it_releases_all_resources : response_deserialization_test_context
    {
        static Stream response_stream;

        Establish context = () =>
        {
            response_stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            Setup("text/plain; charset=utf-8", response_stream);
            http_response_stream.ReadByte();
        };

        Because of =
            () => http_response_stream.Dispose();

        It should_close_the_underlying_web_response =
            () => mock_response.Verify(x => x.Close(), Times.AtLeastOnce());

        It should_dispose_the_response_stream =
            () => (Catch.Exception(() => response_stream.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
    }

    [Subject(typeof(HttpResponseStream))]
    class when_http_response_is_newed_with_null_request
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpResponseStream.InitializeResponseStream(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_that_the_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldContain("request");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_http_response_is_newed_and_the_stream_is_null
    {
        static Mock<WebRequest> mock_request;
        static Mock<WebResponse> mock_response;
        static Exception exception;

        Establish context = () =>
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns((Stream)null);
            mock_response.Object.ContentType = "plain/text; charset=utf-8";
            mock_response.Object.ContentLength = 0;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
        };

        Because of =
            () => exception = Catch.Exception(() => HttpResponseStream.InitializeResponseStream(mock_request.Object));

        It should_throw_exception =
            () => exception.ShouldBeOfType<Exception>();

        It should_report_that_the_response_stream_is_null =
            () => exception.Message.ShouldContain("Response stream is null");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_http_response_is_used_with_byte_array_data : response_deserialization_test_context
    {
        Establish context = 
            () => Setup("application/octet-stream", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

        It should_return_all_byte_array_data =
            () => Encoding.UTF8.GetString(http_response_stream.AsByteArray()).ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_reading_with_plain_text_data_with_defined_charset_as_string : response_deserialization_test_context
    {
        Establish context =
            () => Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

        It should_deserialize_string_data =
            () => http_response_stream.AsString().ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_reading_with_plain_text_data_without_defined_charset_as_string : response_deserialization_test_context
    {
        Establish context =
            () => Setup("text/plain", new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes("Hello World!")));

        It should_deserialize_string_data_assuming_iso_8859_1 =
            () => http_response_stream.AsString().ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_reading_with_plain_text_data_without_defined_charset_as_string_and_overriding_encoding  : response_deserialization_test_context
    {
        Establish context =
            () => Setup("text/plain", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

        It should_deserialize_string_data_using_provided_encoding =
            () => http_response_stream.AsString(Encoding.UTF32).ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_reading_with_plain_text_data_with_defined_charset_as_string_using_specific_matching_encoding : response_deserialization_test_context
    {
        Establish context =
            () => Setup("text/plain; charset=utf-32", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

        It should_deserialize_string_data =
            () => http_response_stream.AsString(Encoding.UTF32).ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponseStream))]
    class when_checking_web_request_properties_on_http_response_stream
    {
        static Mock<WebRequest> mock_request;
        static Mock<WebResponse> mock_response;
        static HttpResponseStream http_response_stream;

        Establish context = () =>
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(new MemoryStream());
            mock_response.Setup(x => x.IsMutuallyAuthenticated).Returns(true);
            mock_response.Object.ContentLength = 42;
            mock_response.Object.ContentType = "text/plain; charset=utf-8";
            mock_response.Setup(x => x.ResponseUri).Returns(new Uri("http://contoso.foo/"));
            mock_response.Setup(x => x.Headers).Returns(new WebHeaderCollection());
            mock_response.Setup(x => x.SupportsHeaders).Returns(true);

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
        };

        Because of =
            () => http_response_stream = HttpResponseStream.InitializeResponseStream(mock_request.Object);

        It should_return_is_mutually_authenticated_from_wrapped_web_response =
            () => http_response_stream.IsMutuallyAuthenticated.ShouldBeTrue();

        It should_return_content_length_from_wrapped_web_response =
            () => http_response_stream.ContentLength.ShouldEqual(42);

        It should_return_media_type_from_wrapped_web_response =
            () => http_response_stream.ContentType.MediaType.ShouldEqual("text/plain");

        It should_return_charset_from_wrapped_web_response =
            () => http_response_stream.ContentType.CharSet.ShouldEqual("utf-8");

        It should_return_response_uri_from_wrapped_web_response =
            () => http_response_stream.ResponseUri.ShouldBeTheSameAs(mock_response.Object.ResponseUri);

        It should_return_headers_from_wrapped_web_response =
            () => http_response_stream.Headers.ShouldBeTheSameAs(mock_response.Object.Headers);

        It should_return_supports_headers_from_wrapped_web_response =
            () => http_response_stream.SupportsHeaders.ShouldBeTrue();
    }

    [Subject(typeof(HttpResponseStream))]
    class when_using_stream_methods_and_properties_on_http_response_stream
    {
        static Mock<WebRequest> mock_request;
        static Mock<WebResponse> mock_response;
        static HttpResponseStream http_response_stream;
        static Mock<Stream> mock_underying_stream;
        static byte[] buffer;

        Establish context = () =>
        {
            buffer = new byte[100];

            mock_underying_stream = new Mock<Stream>();
            mock_underying_stream.SetupAllProperties();

            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(mock_underying_stream.Object);

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);

            http_response_stream = HttpResponseStream.InitializeResponseStream(mock_request.Object);
        };

        Because of =
            () => CallAllOverriddenStreamMethodsAndProperties();

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        static void CallAllOverriddenStreamMethodsAndProperties()
        {
            // ReSharper disable UnusedVariable
            #pragma warning disable 168

            var canRead = http_response_stream.CanRead;
            var canSeek = http_response_stream.CanSeek;
            var canWrite = http_response_stream.CanWrite;
            var canTimeout = http_response_stream.CanTimeout;
            var length = http_response_stream.Length;

            var position = http_response_stream.Position;
            http_response_stream.Position = 111;

            var readTimeout = http_response_stream.ReadTimeout;
            http_response_stream.ReadTimeout = 222;

            var writeTimeout = http_response_stream.WriteTimeout;
            http_response_stream.WriteTimeout = 333;

            http_response_stream.Flush();
            http_response_stream.Seek(444, SeekOrigin.Current);
            http_response_stream.SetLength(555);
            http_response_stream.Read(buffer, 666, 777);
            http_response_stream.Write(buffer, 888, 999);

            #pragma warning restore 168
            // ReSharper restore UnusedVariable
        }

        It should_call_can_read_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.CanRead, Times.AtLeastOnce());

        It should_call_can_seek_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.CanSeek, Times.AtLeastOnce());

        It should_call_can_write_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.CanWrite, Times.AtLeastOnce());

        It should_call_can_timeout_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.CanTimeout, Times.AtLeastOnce());

        It should_call_length_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.Length, Times.AtLeastOnce());

        It should_call_get_position_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.Position, Times.AtLeastOnce());

        It should_call_set_position_on_the_undelying_stream =
            () => mock_underying_stream.VerifySet(x => x.Position = 111, Times.AtLeastOnce());

        It should_call_get_read_timeout_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.ReadTimeout, Times.AtLeastOnce());

        It should_call_set_read_timeout_on_the_undelying_stream =
            () => mock_underying_stream.VerifySet(x => x.ReadTimeout = 222, Times.AtLeastOnce());

        It should_call_get_write_timeout_on_the_undelying_stream =
            () => mock_underying_stream.VerifyGet(x => x.WriteTimeout, Times.AtLeastOnce());

        It should_call_set_write_timeout_on_the_undelying_stream =
            () => mock_underying_stream.VerifySet(x => x.WriteTimeout = 333, Times.AtLeastOnce());

        It should_call_flush_on_the_undelying_stream =
            () => mock_underying_stream.Verify(x => x.Flush(), Times.AtLeastOnce());

        It should_call_seek_on_the_undelying_stream =
            () => mock_underying_stream.Verify(x => x.Seek(444, SeekOrigin.Current), Times.AtLeastOnce());

        It should_call_set_length_on_the_undelying_stream =
            () => mock_underying_stream.Verify(x => x.SetLength(555), Times.AtLeastOnce());

        It should_call_read_on_the_undelying_stream =
            () => mock_underying_stream.Verify(x => x.Read(buffer, 666, 777), Times.AtLeastOnce());

        It should_call_write_on_the_undelying_stream =
            () => mock_underying_stream.Verify(x => x.Write(buffer, 888, 999), Times.AtLeastOnce());
    }
}
