using System.Collections.Generic;
using System.Net;

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

        /// <summary>
        /// Adds supported decoders into accept-encoding header of the request.
        /// </summary>
        void AddAcceptEncodings(WebRequest request);
    }
}