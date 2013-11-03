using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines a decoder factory. By default the factory is populated by decoders that use 
    /// standard .Net GZipStream and DeflateStream for gzip/x-gzip/deflate encodings.
    /// All class members are thread safe.
    /// </summary>
    public interface IContentDecoderFactory
    {
        /// <summary>
        /// Gets a decoder for the specified content encoding.
        /// </summary>
        IDecodeContent Get(string encoding);

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        ICollection<string> GetSupportedEncodings();
    }
}