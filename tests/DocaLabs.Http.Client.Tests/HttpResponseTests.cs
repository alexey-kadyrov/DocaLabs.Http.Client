using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests
{
    public class HttpResponseInBodyTestClass
    {
        public int Value1 { get; set; }
        public string Value2 { get; set; }
    }

    [Subject(typeof(HttpResponse))]
    class when_http_response_is_disposed_it_releases_all_resources : response_deserialization_test_context
    {
        static Stream response_stream;

        Establish context = () =>
        {
            response_stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            Setup("text/plain; charset=utf-8", response_stream);
        };

        Because of =
            () => http_response.Dispose();

        It should_close_the_underlying_web_response =
            () => mock_response.Verify(x => x.Close(), Times.AtLeastOnce());

        It should_dispose_the_response_stream =
            () => (Catch.Exception(() => response_stream.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
    }

    [Subject(typeof(HttpResponse))]
    class when_http_response_is_newed_with_null_request
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new HttpResponse(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_that_the_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldContain("request");
    }

    [Subject(typeof(HttpResponse))]
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
            () => exception = Catch.Exception(() => new HttpResponse(mock_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_report_that_the_response_stream_is_null =
            () => exception.Message.ShouldContain("Response stream is null");
    }

    [Subject(typeof(HttpResponse))]
    class when_http_response_is_used_with_byte_array_data : response_deserialization_test_context
    {
        Establish context = 
            () => Setup("application/octet-stream", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

        It should_return_all_byte_array_data =
            () => Encoding.UTF8.GetString(http_response.AsByteArray()).ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponse))]
    class when_http_response_is_used_with_plain_text_data : response_deserialization_test_context
    {
        Establish context =
            () => Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

        It should_deserialize_string_data =
            () => http_response.AsString().ShouldEqual("Hello World!");
    }

    [Subject(typeof(HttpResponse))]
    class when_http_response_is_newed_for_request
    {
        static Mock<WebRequest> mock_request;
        static Mock<WebResponse> mock_response;
        static HttpResponse http_response;

        Establish context = () =>
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(new MemoryStream());
            mock_response.Setup(x => x.IsMutuallyAuthenticated).Returns(true);
            mock_response.Object.ContentLength = 42;
            mock_response.Object.ContentType = "plain/text; charset=utf-8";
            mock_response.Setup(x => x.ResponseUri).Returns(new Uri("http://contoso.foo/"));
            mock_response.Setup(x => x.Headers).Returns(new WebHeaderCollection());
            mock_response.Setup(x => x.SupportsHeaders).Returns(true);

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
        };

        Because of =
            () => http_response = new HttpResponse(mock_request.Object);

        It should_return_is_mutually_authenticated_from_wrapped_web_response =
            () => http_response.IsMutuallyAuthenticated.ShouldBeTrue();

        It should_return_content_length_from_wrapped_web_response =
            () => http_response.ContentLength.ShouldEqual(42);

        It should_return_media_type_from_wrapped_web_response =
            () => http_response.ContentType.MediaType.ShouldEqual("plain/text");

        It should_return_charset_from_wrapped_web_response =
            () => http_response.ContentType.CharSet.ShouldEqual("utf-8");

        It should_return_response_uri_from_wrapped_web_response =
            () => http_response.ResponseUri.ShouldBeTheSameAs(mock_response.Object.ResponseUri);

        It should_return_headers_from_wrapped_web_response =
            () => http_response.Headers.ShouldBeTheSameAs(mock_response.Object.Headers);

        It should_return_supports_headers_from_wrapped_web_response =
            () => http_response.SupportsHeaders.ShouldBeTrue();
    }
}
