using System.IO;
using System.IO.Compression;
using System.Text;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.ContentEncoding
{
    [Subject(typeof(DeflateContentDecoder))]
    class when_deflate_decoder_is_used
    {
        static DeflateContentDecoder decoder;
        static MemoryStream comressed_stream;
        static Stream decompression_stream;

        Cleanup after_each =
            () => decompression_stream.Dispose();

        Establish context = () =>
        {
            decoder = new DeflateContentDecoder();

            using (var comressedStream = new MemoryStream())
            {
                using (var uncomressedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")))
                using (var compressionStream = new DeflateStream(comressedStream, CompressionMode.Compress))
                {
                    uncomressedStream.CopyTo(compressionStream);
                }

                comressed_stream = new MemoryStream(comressedStream.ToArray());
            }
        };

        Because of =
            () => decompression_stream = decoder.GetDecompressionStream(comressed_stream);

        It should_be_able_to_decomress =
            () => DecomressData().ShouldEqual("Hello World!");

        static string DecomressData()
        {
            using (var data = new MemoryStream())
            {
                using (decompression_stream)
                {
                    decompression_stream.CopyTo(data);
                }

                return Encoding.UTF8.GetString(data.ToArray());
            }
        }
    }
}
