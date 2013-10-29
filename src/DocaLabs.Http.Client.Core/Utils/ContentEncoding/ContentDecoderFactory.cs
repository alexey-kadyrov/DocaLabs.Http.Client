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
    public static class ContentDecoderFactory
    {
        static readonly ConcurrentDictionary<string, IDecodeContent> Decoders;

        static ContentDecoderFactory()
        {
            Decoders = new ConcurrentDictionary<string, IDecodeContent>(StringComparer.OrdinalIgnoreCase);

            Decoders[KnownContentEncodings.Gzip] = new GZipContentDecoder();
            Decoders[KnownContentEncodings.XGzip] = new GZipContentDecoder();
            Decoders[KnownContentEncodings.Deflate] = new DeflateContentDecoder();
        }

        /// <summary>
        /// Gets a decoder for the specified content encoding.
        /// </summary>
        static public IDecodeContent Get(string encoding)
        {
            if (string.IsNullOrWhiteSpace(encoding))
                throw new ArgumentNullException("encoding");

            IDecodeContent decoder;
            if (Decoders.TryGetValue(encoding, out decoder) && decoder != null)
                return decoder;

            throw new ArgumentException(string.Format(Text.compression_format_is_not_suppoerted, encoding), "encoding");
        }

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        static public ICollection<string> GetSupportedEncodings()
        {
            return Decoders.Keys;
        }

        /// <summary>
        /// Adds or replaces existing decoder.
        /// </summary>
        static public void AddOrReplace(string encoding, IDecodeContent decoder)
        {
            Decoders.AddOrUpdate(encoding, k => decoder, (k, v) => decoder);
        }

        /// <summary>
        /// Removes a decoder. If the decoder doesn't exist no exception is thrown.
        /// </summary>
        static public void Remove(string encoding)
        {
            IDecodeContent existingDecoder;
            Decoders.TryRemove(encoding, out existingDecoder);
        }

        /// <summary>
        /// Adds supported decoders into accept-encoding header of the request.
        /// </summary>
        static public void AddAcceptEncodings(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (var decoder in GetSupportedEncodings())
                request.Headers.Add("Accept-Encoding", decoder);
        }
    }
}