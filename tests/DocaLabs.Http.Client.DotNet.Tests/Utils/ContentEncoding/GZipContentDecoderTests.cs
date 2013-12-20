using System.IO;
using System.IO.Compression;
using System.Text;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [TestClass]
    public class when_gzip_decoder_is_used
    {
        static GZipContentDecoder _decoder;
        static MemoryStream _comressedStream;
        static Stream _decompressionStream;

        [ClassCleanup]
        public static void Cleanup()
        {
            _decompressionStream.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _decoder = new GZipContentDecoder();

            using(var comressedStream = new MemoryStream())
            {
                using (var uncomressedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")))
                using (var compressionStream = new GZipStream(comressedStream, CompressionMode.Compress))
                {
                    uncomressedStream.CopyTo(compressionStream);
                }

                _comressedStream = new MemoryStream(comressedStream.ToArray());
            }

            BecauseOf();
        }

        static void BecauseOf()
        {
            _decompressionStream = _decoder.GetDecompressionStream(_comressedStream);
        }

        [TestMethod]
        public void it_should_be_able_to_decompress()
        {
            DecomressData().ShouldEqual("Hello World!");
        }

        static string DecomressData()
        {
            using (var data = new MemoryStream())
            {
                using (_decompressionStream)
                {
                    _decompressionStream.CopyTo(data);
                }

                return Encoding.UTF8.GetString(data.ToArray());
            }
        }
    }
}
