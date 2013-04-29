using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DocaLabs.Http.Client.Resources;

namespace DocaLabs.Http.Client.Binding.ContentEncoding
{
    /// <summary>
    /// Defines an encoder factory. By default the factory is populated by encoders that use
    /// standard .Net GZipStream and DeflateStream for gzip/x-gzip/deflate encodings.
    /// All class members are thread safe.
    /// </summary>
    public static class ContentEncoderFactory
    {
        static readonly ConcurrentDictionary<string, IEncodeContent> Encoders;

        static ContentEncoderFactory()
        {
            Encoders = new ConcurrentDictionary<string, IEncodeContent>(StringComparer.OrdinalIgnoreCase);

            Encoders[KnownContentEncodings.Gzip] = new GZipContentEncoder();
            Encoders[KnownContentEncodings.XGzip] = new GZipContentEncoder();
            Encoders[KnownContentEncodings.Deflate] = new DeflateContentEncoder();
        }

        /// <summary>
        /// Gets a encoder for the specified content encoding.
        /// </summary>
        static public IEncodeContent Get(string encoding)
        {
            if (string.IsNullOrWhiteSpace(encoding))
                throw new ArgumentNullException("encoding");

            IEncodeContent encoder;
            if (Encoders.TryGetValue(encoding, out encoder) && encoder != null)
                return encoder;

            throw new ArgumentException(string.Format(Text.compression_format_is_not_suppoerted, encoding), "encoding");
        }

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        static public ICollection<string> GetSupportedEncodings()
        {
            return Encoders.Keys;
        }

        /// <summary>
        /// Adds or replaces existing encoder.
        /// </summary>
        static public void AddOrReplace(string encoding, IEncodeContent encoder)
        {
            Encoders.AddOrUpdate(encoding, k => encoder, (k, v) => encoder);
        }

        /// <summary>
        /// Removes an encoder. If the encoder doesn't exist no exception is thrown.
        /// </summary>
        static public void Remove(string encoding)
        {
            IEncodeContent existingEncoder;
            Encoders.TryRemove(encoding, out existingEncoder);
        }
    }
}
