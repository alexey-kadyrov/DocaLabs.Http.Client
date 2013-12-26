using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [TestClass]
    public class when_http_response_is_initialized_but_not_used_and_then_disposed_it_releases_all_resources
    {
        static Stream _testData;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _testData = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-8", _testData);

            BecauseOf();
        }

        static void BecauseOf()
        {
            _helper.ResponseStream.Dispose();
        }

        [TestMethod]
        public void it_should_close_the_underlying_web_response()
        {
            _helper.MockResponse.TestCloseCounter.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_dispose_the_underlying_response_stream()
        {
            (Catch.Exception(() => _testData.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
        }
    }

    [TestClass]
    public class when_asynchronous_http_response_is_initialized_but_not_used_and_then_disposed_it_releases_all_resources
    {
        static Stream _testData;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _testData = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-8", _testData);

            BecauseOf();
        }

        static void BecauseOf()
        {
            _helper.ResponseStream.Dispose();
        }

        [TestMethod]
        public void it_should_close_the_underlying_web_response()
        {
            _helper.MockResponse.TestCloseCounter.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_dispose_the_underlying_response_stream()
        {
            (Catch.Exception(() => _testData.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
        }
    }

    [TestClass]
    public class when_http_response_is_initialized_and_used_and_then_disposed_it_releases_all_resources
    {
        static Stream _testData;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _testData = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-8", _testData);
            _helper.ResponseStream.ReadByte();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _helper.ResponseStream.Dispose();
        }

        [TestMethod]
        public void it_should_close_the_underlying_web_response()
        {
            _helper.MockResponse.TestCloseCounter.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_dispose_the_underlying_response_stream()
        {
            (Catch.Exception(() => _testData.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
        }
    }

    [TestClass]
    public class when_asynchronous_http_response_is_initialized_and_used_and_then_disposed_it_releases_all_resources
    {
        static Stream _testData;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _testData = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-8", _testData);
            _helper.ResponseStream.ReadByte();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _helper.ResponseStream.Dispose();
        }

        [TestMethod]
        public void it_should_close_the_underlying_web_response()
        {
            _helper.MockResponse.TestCloseCounter.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_dispose_the_underlying_response_stream()
        {
            (Catch.Exception(() => _testData.ReadByte()) as ObjectDisposedException).ShouldNotBeNull();
        }
    }

    [TestClass]
    public class when_http_response_is_used_with_byte_array_data
    {
        static HttpResponseStreamHelper _helper;
        static byte[] _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.Setup("application/octet-stream", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsByteArray();
        }

        [TestMethod]
        public void it_should_return_all_byte_array_data()
        {
            Encoding.UTF8.GetString(_result).ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_http_response_is_used_asynchronously_with_byte_array_data
    {
        static HttpResponseStreamHelper _helper;
        static byte[] _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.SetupAsync("application/octet-stream", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsByteArrayAsync(CancellationToken.None).Result;
        }

        [TestMethod]
        public void it_should_return_all_byte_array_data()
        {
            Encoding.UTF8.GetString(_result).ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_reading_with_plain_text_data_with_defined_charset_as_string
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsString();
        }

        [TestMethod]
        public void it_should_deserialize_string_data()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_asynchronously_reading_with_plain_text_data_with_defined_charset_as_string
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsStringAsync().Result;
        }

        [TestMethod]
        public void it_should_deserialize_string_data()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_reading_with_plain_text_data_without_defined_charset_as_string
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.Setup("text/plain", new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsString();
        }

        [TestMethod]
        public void it_should_deserialize_string_data_assuming_iso_8859_1()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_asynchronously_reading_with_plain_text_data_without_defined_charset_as_string
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain", new MemoryStream(Encoding.GetEncoding("ISO-8859-1").GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsStringAsync().Result;
        }

        [TestMethod]
        public void it_should_deserialize_string_data_assuming_iso_8859_1()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_reading_with_plain_text_data_without_defined_charset_as_string_and_overriding_encoding
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.Setup("text/plain", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsString(Encoding.UTF32);
        }

        [TestMethod]
        public void it_should_deserialize_string_data_using_provided_encoding()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_asynchronously_reading_with_plain_text_data_without_defined_charset_as_string_and_overriding_encoding
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsStringAsync(Encoding.UTF32).Result;
        }

        [TestMethod]
        public void it_should_deserialize_string_data_using_provided_encoding()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_reading_with_plain_text_data_with_defined_charset_as_string_using_specific_matching_encoding
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-32", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsString(Encoding.UTF32);
        }

        [TestMethod]
        public void it_should_deserialize_string_data()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_asynchronously_reading_with_plain_text_data_with_defined_charset_as_string_using_specific_matching_encoding
    {
        static HttpResponseStreamHelper _helper;
        static string _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-32", new MemoryStream(Encoding.UTF32.GetBytes("Hello World!")));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _helper.ResponseStream.AsStringAsync(Encoding.UTF32).Result;
        }

        [TestMethod]
        public void it_should_deserialize_string_data()
        {
            _result.ShouldEqual("Hello World!");
        }
    }

    [TestClass]
    public class when_checking_web_request_properties_on_http_response_stream
    {
        static Stream _dataSource;
        static Uri _url;
        static WebHeaderCollection _headers;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _dataSource = new MemoryStream(Encoding.UTF32.GetBytes("Hello World!"));
            _url = new Uri("http://contoso.foo/");
            _headers = new WebHeaderCollection();

            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-8", _dataSource);
            _helper.MockResponse.SetIsMutuallyAuthenticated(true);
            _helper.MockResponse.SetResponseUri(_url);
            _helper.MockResponse.SetHeaders(_headers);
        }

        [TestMethod]
        public void it_should_return_is_mutually_authenticated_from_wrapped_web_response()
        {
            _helper.ResponseStream.IsMutuallyAuthenticated.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_return_content_length_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentLength.ShouldEqual(_dataSource.Length);
        }

        [TestMethod]
        public void it_should_return_media_type_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentType.MediaType.ShouldEqual("text/plain");
        }

        [TestMethod]
        public void it_should_return_charset_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentType.CharSet.ShouldEqual("utf-8");
        }

        [TestMethod]
        public void it_should_return_response_uri_from_wrapped_web_response()
        {
           _helper.ResponseStream.ResponseUri.ShouldBeTheSameAs(_url);
        }

        [TestMethod]
        public void it_should_return_headers_from_wrapped_web_response()
        {
            _helper.ResponseStream.Headers.ShouldBeTheSameAs(_headers);
        }

        [TestMethod]
        public void it_should_return_supports_headers_from_wrapped_web_response()
        {
            _helper.ResponseStream.SupportsHeaders.ShouldBeTrue();
        }
    }

    [TestClass]
    public class when_checking_asynchronous_web_request_properties_on_http_response_stream
    {
        static Stream _dataSource;
        static Uri _url;
        static WebHeaderCollection _headers;
        static HttpResponseStreamHelper _helper;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _dataSource = new MemoryStream(Encoding.UTF32.GetBytes("Hello World!"));
            _url = new Uri("http://contoso.foo/");
            _headers = new WebHeaderCollection();

            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-8", _dataSource);
            _helper.MockResponse.SetIsMutuallyAuthenticated(true);
            _helper.MockResponse.SetResponseUri(_url);
            _helper.MockResponse.SetHeaders(_headers);
        }

        [TestMethod]
        public void it_should_return_is_mutually_authenticated_from_wrapped_web_response()
        {
            _helper.ResponseStream.IsMutuallyAuthenticated.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_return_content_length_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentLength.ShouldEqual(_dataSource.Length);
        }

        [TestMethod]
        public void it_should_return_media_type_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentType.MediaType.ShouldEqual("text/plain");
        }

        [TestMethod]
        public void it_should_return_charset_from_wrapped_web_response()
        {
            _helper.ResponseStream.ContentType.CharSet.ShouldEqual("utf-8");
        }

        [TestMethod]
        public void it_should_return_response_uri_from_wrapped_web_response()
        {
            _helper.ResponseStream.ResponseUri.ShouldBeTheSameAs(_url);
        }

        [TestMethod]
        public void it_should_return_headers_from_wrapped_web_response()
        {
            _helper.ResponseStream.Headers.ShouldBeTheSameAs(_headers);
        }

        [TestMethod]
        public void it_should_return_supports_headers_from_wrapped_web_response()
        {
            _helper.ResponseStream.SupportsHeaders.ShouldBeTrue();
        }
    }
}
