using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines an encoder factory. By default the factory is populated by encoders that use
    /// standard .Net GZipStream and DeflateStream for gzip/x-gzip/deflate encodings.
    /// All class members are thread safe.
    /// </summary>
    public interface IContentEncoderFactory
    {
        /// <summary>
        /// Gets a encoder for the specified content encoding.
        /// </summary>
        IEncodeContent Get(string encoding);

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        ICollection<string> GetSupportedEncodings();
    }
}
