using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [TestClass]
    public class when_using_content_decoder_factory_in_default_configuration
    {
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory = new ContentDecoderFactory();
        }

        [TestMethod]
        public void it_should_return_three_supported_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_decoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder()
        {
            Catch.Exception(() => _factory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }
    }

    [TestClass]
    public class when_getting_decoder_for_unknown_content_encoding
    {
        static Exception _exception;
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _factory.Get("some-wacky-encoding"));
        }

        [TestMethod]
        public void it_should_throw_argument_exception()
        {
            _exception.ShouldBeOfType<ArgumentException>();
        }

        [TestMethod]
        public void it_should_report_encoding_argument()
        {
            ((ArgumentException) _exception).ParamName.ShouldEqual("encoding");
        }
    }

    [TestClass]
    public class when_getting_decoder_for_null_content_encoding
    {
        static Exception _exception;
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _factory.Get(null));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_encoding_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("encoding");
        }
    }

    [TestClass]
    public class when_adding_accept_encodings_header_in_default_configuration
    {
        static Mock<WebRequest> _request;
        static WebHeaderCollection _headers;
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _headers = new WebHeaderCollection();

            _request = new Mock<WebRequest>();
            _request.SetupAllProperties();
            _request.Setup(x => x.Headers).Returns(_headers);

            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.TransferAcceptEncodings(_request.Object);
        }

        [TestMethod]
        public void it_should_add_all_supported_content_encodings_into_header()
        {
            _headers["accept-encoding"].Split(',').ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }
    }

    [TestClass]
    public class when_adding_accept_encodings_header_to_null_web_request
    {
        static Exception _exception;
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecuaseOf();
        }

        static void BecuaseOf()
        {
            _exception = Catch.Exception(() => _factory.TransferAcceptEncodings(null));
        }

        [TestMethod]
        public void it_should_throw_argumant_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_request_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("request");
        }
    }

    [TestClass]
    public class when_removing_decoder
    {
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.Remove(KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_remaining_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip);
        }

        [TestMethod]
        public void it_should_return_decoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_removed_content_encoding_when_getting_decoder()
        {
            Catch.Exception(() => _factory.Get(KnownContentEncodings.XGzip)).ShouldBeOfType<ArgumentException>();
        }
    }

    [TestClass]
    public class when_removing_non_existent_decoder
    {
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.Remove("some-wacky-encoding-2");
        }

        [TestMethod]
        public void it_should_return_all_original_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_decoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder()
        {
            Catch.Exception(() => _factory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }
    }

    [TestClass]
    public class when_adding_new_decoder
    {
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.AddOrReplace("some-wacky-encoding", new TestDecoder());
        }

        [TestMethod]
        public void it_should_return_remaining_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip, "some-wacky-encoding");
        }

        [TestMethod]
        public void it_should_return_decoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_new_content_encoding()
        {
            _factory.Get("some-wacky-encoding").ShouldBeOfType<TestDecoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder()
        {
            Catch.Exception(() => _factory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [TestClass]
    public class when_replacing_existing_decoder
    {
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.AddOrReplace(KnownContentEncodings.XGzip, new TestDecoder());
        }

        [TestMethod]
        public void it_should_return_remaining_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_decoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_decoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
        }

        [TestMethod]
        public void it_should_return_new_decoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<TestDecoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder()
        {
            Catch.Exception(() => _factory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [TestClass]
    public class when_content_decoder_factory_is_used_concurrently
    {
        static int _count;
        static object _locker;
        static Random _random;
        static ContentDecoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _count = 0;
            _locker = new object();
            _random = new Random();

            _factory = new ContentDecoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            Parallel.For(0, 1000000, i =>
            {
                int ii;

                lock (_locker)
                {
                    ii = _random.Next(6);
                }

                switch (ii)
                {
                    case 0:
                    {
                        var name = "some-wacky-encoding" + ii;
                        _factory.AddOrReplace(name, new TestDecoder());
                        _factory.Remove(name);
                        break;
                    }
                    case 1:
                        _factory.GetSupportedEncodings().Count.ShouldBeEqualOrGreaterThan(3);
                        break;
                    case 2:
                        _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
                        break;
                    case 3:
                        _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
                        break;
                    case 4:
                        _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();
                        break;
                    case 5:
                        Catch.Exception(() => _factory.Get("some-other-wacky-encoding-42")).ShouldBeOfType<ArgumentException>();
                        break;
                }

                Interlocked.Increment(ref _count);
            });
        }

        [TestMethod]
        public void it_should_execute_all_operations()
        {
            _count.ShouldEqual(1000000);
        }

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }
}
