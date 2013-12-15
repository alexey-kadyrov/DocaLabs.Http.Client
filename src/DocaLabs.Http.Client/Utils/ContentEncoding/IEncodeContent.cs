using System.IO;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines methods to get an encoder stream for the http content.
    /// </summary>
    public interface IEncodeContent
    {
        /// <summary>
        /// Returns stream that can be used to compress data.
        /// </summary>
        Stream GetCompressionStream(Stream stream);
    }
}