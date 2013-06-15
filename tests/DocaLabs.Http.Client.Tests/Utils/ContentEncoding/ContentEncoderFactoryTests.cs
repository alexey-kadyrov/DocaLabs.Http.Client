using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [Subject(typeof(ContentEncoderFactory))]
    class when_using_in_default_configuration
    {
        It should_return_three_supported_content_encodings = () => ContentEncoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_encoder_for_deflate_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();

        It should_return_encoder_for_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();

        It should_return_encoder_for_x_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder =
            () => Catch.Exception(() => ContentEncoderFactory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        It should_report_encoding_argument_for_unknown_content_encoding_when_getting_encoder =
            () => ((ArgumentException)Catch.Exception(() => ContentEncoderFactory.Get("some-wacky-encoding"))).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_getting_encoder_for_unknown_content_encoding
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ContentEncoderFactory.Get("some-wacky-encoding"));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_encoding_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_getting_encoder_for_null_content_encoding
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ContentEncoderFactory.Get(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_encoding_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("encoding");
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_removing_encoder
    {
        Cleanup after_each =
            () => ContentEncoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new GZipContentEncoder());

        Because of =
            () => ContentEncoderFactory.Remove(KnownContentEncodings.XGzip);

        It should_return_remaining_content_encodings = () => ContentEncoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip);

        It should_return_encoder_for_deflate_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();

        It should_return_encoder_for_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();

        It should_throw_argument_exception_for_removed_content_encoding_when_getting_encoder =
            () => Catch.Exception(() => ContentEncoderFactory.Get(KnownContentEncodings.XGzip)).ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_removing_non_existent_encoder
    {
        Because of =
            () => ContentEncoderFactory.Remove("some-wacky-encoding");

        It should_return_all_original_content_encodings = () => ContentEncoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_encoder_for_deflate_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();

        It should_return_encoder_for_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();

        It should_return_encoder_for_x_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder =
            () => Catch.Exception(() => ContentEncoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_adding_new_encoder
    {
        Cleanup after_each =
            () => ContentEncoderFactory.Remove("some-wacky-encoding");

        Because of =
            () => ContentEncoderFactory.AddOrReplace("some-wacky-encoding", new TestEncoder());

        It should_return_remaining_content_encodings = () => ContentEncoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip, "some-wacky-encoding");

        It should_return_encoder_for_deflate_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();

        It should_return_encoder_for_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();

        It should_return_encoder_for_x_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();

        It should_return_encoder_for_new_content_encoding =
            () => ContentEncoderFactory.Get("some-wacky-encoding").ShouldBeOfType<TestEncoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder =
            () => Catch.Exception(() => ContentEncoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_replacing_existing_encoder
    {
        Cleanup after_each =
            () => ContentEncoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new GZipContentEncoder());

        Because of =
            () => ContentEncoderFactory.AddOrReplace(KnownContentEncodings.XGzip, new TestEncoder());

        It should_return_remaining_content_encodings = () => ContentEncoderFactory.GetSupportedEncodings()
            .ShouldContainOnly(KnownContentEncodings.Deflate, KnownContentEncodings.Gzip, KnownContentEncodings.XGzip);

        It should_return_encoder_for_deflate_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();

        It should_return_encoder_for_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();

        It should_return_new_encoder_for_x_gzip_content_encoding =
            () => ContentEncoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<TestEncoder>();

        It should_throw_argument_exception_for_unknown_content_encoding_when_getting_encoder =
            () => Catch.Exception(() => ContentEncoderFactory.Get("some-wacky-encoding")).ShouldBeOfType<ArgumentException>();

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }

    [Subject(typeof(ContentEncoderFactory))]
    class when_content_encoder_factory_is_used_concurrently
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
                    ContentEncoderFactory.AddOrReplace("some-wacky-encoding", new TestEncoder());
                    ContentEncoderFactory.Remove("some-wacky-encoding");
                    break;
                case 1:
                    ContentEncoderFactory.GetSupportedEncodings().Count.ShouldBeGreaterThanOrEqualTo(3);
                    break;
                case 2:
                    ContentEncoderFactory.Get(KnownContentEncodings.Deflate).ShouldBeOfType<DeflateContentEncoder>();
                    break;
                case 3:
                    ContentEncoderFactory.Get(KnownContentEncodings.Gzip).ShouldBeOfType<GZipContentEncoder>();
                    break;
                case 4:
                    ContentEncoderFactory.Get(KnownContentEncodings.XGzip).ShouldBeOfType<GZipContentEncoder>();
                    break;
                case 5:
                    Catch.Exception(() => ContentEncoderFactory.Get("some-other-wacky-encoding")).ShouldBeOfType<ArgumentException>();
                    break;
            }

            Interlocked.Increment(ref count);
        });

        It should_execute_all_operations =
            () => count.ShouldEqual(1000000);

        class TestEncoder : IEncodeContent
        {
            public Stream GetCompressionStream(Stream stream)
            {
                return new MemoryStream();
            }
        }
    }
}
