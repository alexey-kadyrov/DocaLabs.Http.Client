using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [TestClass]
    public class when_instantiatin_encoder_factory_in_default_configuration
    {
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory = new ContentEncoderFactory();
        }

        [TestMethod]
        public void it_should_return_three_supported_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_encoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder()
        {
            Catch.Exception(() => _factory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }

        [TestMethod]
        public void it_should_report_encoding_argument_for_unknown_content_encoding_when_getting_encoder()
        {
            ((ArgumentException) Catch.Exception(() => _factory.Get("some-wacky-encoding"))).ParamName.ShouldEqual("encoding");
        }
    }

    [TestClass]
    public class when_getting_encoder_for_unknown_content_encoding
    {
        static Exception _exception;
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

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
    public class when_getting_encoder_for_null_content_encoding
    {
        static Exception _exception;
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

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
    public class when_removing_encoder
    {
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

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
        public void it_should_return_encoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_removed_content_encoding_when_getting_encoder()
        {
            Catch.Exception(() => _factory.Get(KnownContentEncodings.XGzip)).ShouldBeOfType<ArgumentException>();
        }
    }

    [TestClass]
    public class when_removing_non_existent_encoder
    {
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.Remove("some-wacky-encoding");
        }

        [TestMethod]
        public void it_should_return_all_original_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_encoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder()
        {
            Catch.Exception(() => _factory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }
    }

    [TestClass]
    public class when_adding_new_encoder
    {
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.AddOrReplace("some-wacky-encoding", new TestEncoder());
        }

        [TestMethod]
        public void it_should_return_remaining_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip, "some-wacky-encoding");
        }

        [TestMethod]
        public void it_should_return_encoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_new_content_encoding()
        {
            _factory.Get("some-wacky-encoding").ShouldBeOfType<TestEncoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder()
        {
            Catch.Exception(() => _factory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [TestClass]
    public class when_replacing_existing_encoder
    {
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _factory = new ContentEncoderFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _factory.AddOrReplace(KnownContentEncodings.XGzip, new TestEncoder());
        }

        [TestMethod]
        public void it_should_return_remaining_content_encodings()
        {
            _factory.GetSupportedEncodings().ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
        }

        [TestMethod]
        public void it_should_return_encoder_for_deflate_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_encoder_for_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
        }

        [TestMethod]
        public void it_should_return_new_encoder_for_x_gzip_content_encoding()
        {
            _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<TestEncoder>();
        }

        [TestMethod]
        public void it_should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder()
        {
            Catch.Exception(() => _factory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();
        }

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [TestClass]
    public class when_content_encoder_factory_is_used_concurrently
    {
        static int _count;
        static object _locker;
        static Random _random;
        static ContentEncoderFactory _factory;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _count = 0;
            _locker = new object();
            _random = new Random();
            _factory = new ContentEncoderFactory();

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
                        _factory.AddOrReplace("some-wacky-encoding", new TestEncoder());
                        _factory.Remove("some-wacky-encoding");
                        break;
                    case 1:
                        _factory.GetSupportedEncodings().Count.ShouldBeEqualOrGreaterThan(3);
                        break;
                    case 2:
                        _factory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
                        break;
                    case 3:
                        _factory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
                        break;
                    case 4:
                        _factory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();
                        break;
                    case 5:
                        Catch.Exception(() => _factory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
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

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }
}
