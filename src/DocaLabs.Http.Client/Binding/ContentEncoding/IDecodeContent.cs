using System.IO;

namespace DocaLabs.Http.Client.Binding.ContentEncoding
{
    /// <summary>
    /// Defines methods to get a decoder stream for the http content.
    /// </summary>
    public interface IDecodeContent
    {
        /// <summary>
        /// Returns stream that can be used to decompress data.
        /// </summary>
        Stream GetDecompressionStream(Stream stream);
    }
}