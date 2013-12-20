using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DocaLabs.Http.Client.Resources;

namespace DocaLabs.Http.Client.Utils.ContentEncoding
{
    /// <summary>
    /// Defines an encoder factory. By default the factory is populated by encoders that use
    /// standard .Net GZipStream and DeflateStream for gzip/x-gzip/deflate encodings.
    /// All class members are thread safe.
    /// </summary>
    public class ContentEncoderFactory : IContentEncoderFactory
    {
        readonly ConcurrentDictionary<string, IEncodeContent> _encoders;

        /// <summary>
        /// Initializes an instance of the ContentEncoderFactory class;
        /// </summary>
        public ContentEncoderFactory()
        {
            _encoders = new ConcurrentDictionary<string, IEncodeContent>(StringComparer.OrdinalIgnoreCase);

            _encoders[KnownContentEncodings.Gzip] = new GZipContentEncoder();
            _encoders[KnownContentEncodings.XGzip] = new GZipContentEncoder();
            _encoders[KnownContentEncodings.Deflate] = new DeflateContentEncoder();
        }

        /// <summary>
        /// Gets a encoder for the specified content encoding.
        /// </summary>
        public IEncodeContent Get(string encoding)
        {
            if (string.IsNullOrWhiteSpace(encoding))
                throw new ArgumentNullException("encoding");

            IEncodeContent encoder;
            if (_encoders.TryGetValue(encoding, out encoder) && encoder != null)
                return encoder;

            throw new ArgumentException(string.Format(PlatformText.compression_format_is_not_suppoerted, encoding), "encoding");
        }

        /// <summary>
        /// Returns list of supported encodings.
        /// </summary>
        public ICollection<string> GetSupportedEncodings()
        {
            return _encoders.Keys;
        }

        /// <summary>
        /// Adds or replaces existing encoder.
        /// </summary>
        public void AddOrReplace(string encoding, IEncodeContent encoder)
        {
            _encoders.AddOrUpdate(encoding, k => encoder, (k, v) => encoder);
        }

        /// <summary>
        /// Removes an encoder. If the encoder doesn't exist no exception is thrown.
        /// </summary>
        public void Remove(string encoding)
        {
            IEncodeContent existingEncoder;
            _encoders.TryRemove(encoding, out existingEncoder);
        }
    }
}
