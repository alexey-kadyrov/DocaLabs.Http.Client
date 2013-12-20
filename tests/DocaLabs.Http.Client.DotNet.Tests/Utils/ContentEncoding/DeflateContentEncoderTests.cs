using System.IO;
using System.IO.Compression;
using System.Text;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Utils.ContentEncoding
{
    [TestClass]
    public class when_deflate_encoder_is_used
    {
        static DeflateContentEncoder _encoder;
        static MemoryStream _comressedStream;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _encoder = new DeflateContentEncoder();

            BecauseOf();
        }

        static void BecauseOf()
        {
            using (var comressedStream = new MemoryStream())
            {
                using (var uncomressedStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")))
                using (var compressionStream = _encoder.GetCompressionStream(comressedStream))
                {
                    uncomressedStream.CopyTo(compressionStream);
                }

                _comressedStream = new MemoryStream(comressedStream.ToArray());
            }
        }

        [TestMethod]

        public void it_should_be_able_to_compress()
        {
            DecomressData().ShouldEqual("Hello World!");
        }

        static string DecomressData()
        {
            using (var data = new MemoryStream())
            {
                using (var decompressionStream = new DeflateStream(_comressedStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(data);
                }

                return Encoding.UTF8.GetString(data.ToArray());
            }
        }
    }
}
