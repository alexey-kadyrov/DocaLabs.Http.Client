using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using DocaLabs.Http.Client.Resources;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines a decoder factory. By default the factory is populated by decoders that use 
    /// standard .Net GZipStream and DeflateStream for gzip/x-gzip/deflate encodings.
    /// All class members are thread safe.
    /// </summary>
    public class ContentDecoderFactory : IContentDecoderFactory
    {
        readonly ConcurrentDictionary<string, IDecodeContent> _decoders;

        /// <summary>
        /// Initializes an instance of the ContentDecoderFactory class with default set of decoders: gzip, x-gzip, and deflate.
        /// </summary>
        public ContentDecoderFactory()
        {
            _decoders = new ConcurrentDictionary<string, IDecodeContent>(StringComparer.OrdinalIgnoreCase);

            _decoders[KnownContentEncodings.Gzip] = new GZipContentDecoder();
            _decoders[KnownContentEncodings.XGzip] = new GZipContentDecoder();
            _decoders[KnownContentEncodings.Deflate] = new DeflateContentDecoder();
        }

        /// <summary>
        /// Gets a decoder for the specified content encoding.
        /// </summary>
        public IDecodeContent Get(string encoding)
        {
            if (string.IsNullOrWhiteSpace(encoding))
                throw new ArgumentNullException("encoding");

            IDecodeContent decoder;
            if (_decoders.TryGetValue(encoding, out decoder) && decoder != null)
                return decoder;

            throw new ArgumentException(string.Format(PlatformText.compression_format_is_not_suppoerted, encoding), "encoding");
        }

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        public ICollection<string> GetSupportedEncodings()
        {
            return _decoders.Keys;
        }

        /// <summary>
        /// Adds supported decoders into accept-encoding header of the request.
        /// </summary>
        public void TransferAcceptEncodings(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (var decoder in GetSupportedEncodings())
                request.Headers.Add("Accept-Encoding", decoder);
        }

        /// <summary>
        /// Adds or replaces existing decoder.
        /// </summary>
        public void AddOrReplace(string encoding, IDecodeContent decoder)
        {
            _decoders.AddOrUpdate(encoding, k => decoder, (k, v) => decoder);
        }

        /// <summary>
        /// Removes a decoder. If the decoder doesn't exist no exception is thrown.
        /// </summary>
        public void Remove(string encoding)
        {
            IDecodeContent existingDecoder;
            _decoders.TryRemove(encoding, out existingDecoder);
        }
    }
}