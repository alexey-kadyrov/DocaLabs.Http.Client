using System.IO;
using System.IO.Compression;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines deflate decoder for the http content.
    /// </summary>
    public class DeflateContentDecoder : IDecodeContent
    {
        /// <summary>
        /// Returns DeflateStream in decompress mode.
        /// </summary>
        public Stream GetDecompressionStream(Stream stream)
        {
            return new DeflateStream(stream, CompressionMode.Decompress);
        }
    }
}