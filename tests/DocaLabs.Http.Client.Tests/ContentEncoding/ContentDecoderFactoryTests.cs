using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.ContentEncoding
{
    [Subject(typeof(ContentDecoderFactory))]
    class when_using_content_decoder_factory_in_default_configuration
    {
        It should_return_three_supported_content_encodings = () => ContentDecoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_decoder_for_deflate_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();

        It should_return_decoder_for_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();

        It should_return_decoder_for_x_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder =
            () => Catch.Exception(() => ContentDecoderFactory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        It should_report_encoding_argument_for_unknown_content_encoding_when_getting_decoder =
            () => ((ArgumentException)Catch.Exception(() => ContentDecoderFactory.Get("some-wacky-encoding"))).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_getting_decoder_for_unknown_content_encoding
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ContentDecoderFactory.Get("some-wacky-encoding"));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_encoding_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_getting_decoder_for_null_content_encoding
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ContentDecoderFactory.Get(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_encoding_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_adding_accept_encodings_header_in_default_configuration
    {
        static Mock<WebRequest> request;
        static WebHeaderCollection headers;

        Establish context = () =>
        {
            headers = new WebHeaderCollection();

            request = new Mock<WebRequest>();
            request.SetupAllProperties();
            request.Setup(x => x.Headers).Returns(headers);
        };

        Because of =
            () => ContentDecoderFactory.AddAcceptEncodings(request.Object);

        It should_add_all_supported_content_encodings_into_header = () => headers["accept-encoding"].Split(',')
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_removing_decoder
    {
        Cleanup after_each =
            () => ContentDecoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new GZipContentDecoder());

        Because of =
            () => ContentDecoderFactory.Remove(KnownContentEncodings.XGzip);

        It should_return_remaining_content_encodings = () => ContentDecoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip);

        It should_return_decoder_for_deflate_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();

        It should_return_decoder_for_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();

        It should_throw_argument_exception_for_removed_content_encoding_when_getting_decoder =
            () => Catch.Exception(() => ContentDecoderFactory.Get(KnownContentEncodings.XGzip)).ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_removing_non_existent_decoder
    {
        Because of =
            () => ContentDecoderFactory.Remove("some-wacky-encoding");

        It should_return_all_original_content_encodings = () => ContentDecoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_decoder_for_deflate_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();

        It should_return_decoder_for_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();

        It should_return_decoder_for_x_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder =
            () => Catch.Exception(() => ContentDecoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_adding_new_decoder
    {
        Cleanup after_each =
            () => ContentDecoderFactory.Remove("some-wacky-encoding");

        Because of =
            () => ContentDecoderFactory.AddOrReplace("some-wacky-encoding", new TestDecoder());

        It should_return_remaining_content_encodings = () => ContentDecoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip, "some-wacky-encoding");

        It should_return_decoder_for_deflate_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();

        It should_return_decoder_for_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();

        It should_return_decoder_for_x_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();

        It should_return_decoder_for_new_content_encoding =
            () => ContentDecoderFactory.Get("some-wacky-encoding").ShouldBeOfType<TestDecoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder =
            () => Catch.Exception(() => ContentDecoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_replacing_existing_decoder
    {
        Cleanup after_each =
            () => ContentDecoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new GZipContentDecoder());

        Because of =
            () => ContentDecoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new TestDecoder());

        It should_return_remaining_content_encodings = () => ContentDecoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_decoder_for_deflate_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();

        It should_return_decoder_for_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();

        It should_return_new_decoder_for_x_gzip_content_encoding =
            () => ContentDecoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<TestDecoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_decoder =
            () => Catch.Exception(() => ContentDecoderFactory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [Subject(typeof(ContentDecoderFactory))]
    class when_content_decoder_factory_is_used_concurrently
    {
        static int count;
        static object locker;
        static Random random;

        Establish context = () =>
        {
            count = 0;
            locker = new object();
            random = new Random();
        };

        Because of = () => Parallel.For(0, 1000000, i =>
        {
            int ii;

            lock (locker)
            {
                ii = random.Next(6);
            }

            switch (ii)
            {
                case 0:
                    ContentDecoderFactory.AddOrReplace("some-wacky-encoding", new TestDecoder());
                    ContentDecoderFactory.Remove("some-wacky-encoding");
                    break;
                case 1:
                    ContentDecoderFactory.GetSupportedEncodings().Count.ShouldBeGreaterThanOrEqualTo(3);
                    break;
                case 2:
                    ContentDecoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentDecoder>();
                    break;
                case 3:
                    ContentDecoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentDecoder>();
                    break;
                case 4:
                    ContentDecoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentDecoder>();
                    break;
                case 5:
                    Catch.Exception(() => ContentDecoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
                    break;
            }

            Interlocked.Increment(ref count);
        });

        It should_execute_all_operations =
            () => count.ShouldEqual(1000000);

        class TestDecoder : IDecodeContent
        {
            public Stream GetDecompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }
}
