using System.IO;
using System.IO.Compression;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines gzip encoder for the http content.
    /// </summary>
    public class GZipContentEncoder : IEncodeContent
    {
        /// <summary>
        /// Returns GZipStream in compress mode.
        /// </summary>
        public Stream GetCompressionStream(Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Compress);
        }
    }
}