using System.IO;
using System.IO.Compression;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines deflate encoder for the http content.
    /// </summary>
    public class DeflateContentEncoder : IEncodeContent
    {
        /// <summary>
        /// Returns DeflateStream in compress mode.
        /// </summary>
        public Stream GetCompressionStream(Stream stream)
        {
            return new DeflateStream(stream, CompressionMode.Compress);
        }
    }
}