using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Wraps around the response stream.
    /// </summary>
    public class HttpResponseStream : Stream
    {
        static readonly Encoding DefaultTextEncoding = Encoding.GetEncoding(CharSets.Iso88591);

        ContentType _contentType;

        internal WebResponse Response { get; set; }

        /// <summary>
        /// Gets or sets the raw response stream.
        /// </summary>
        protected Stream RawResponseStream { get; set; }

        Stream _dataStream;

        /// <summary>
        /// Returns the response stream, if the content is encoded (compressed) then it will be decoded using decoder provided by ContentDecoderFactory.
        /// </summary>
        protected Stream DataStream
        {
            get
            {
                if (_dataStream != null)
                    return _dataStream;

                var httpResponse = Response as HttpWebResponse;
                if (httpResponse == null || string.IsNullOrWhiteSpace(httpResponse.ContentEncoding))
                    return (_dataStream = RawResponseStream);

                return (_dataStream = ContentDecoderFactory.Get(httpResponse.ContentEncoding).GetDecompressionStream(RawResponseStream));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether mutual authentication occurred.
        /// </summary>
        public bool IsMutuallyAuthenticated { get { return Response.IsMutuallyAuthenticated; } }

        /// <summary>
        /// Gets the content length of data being received.
        /// </summary>
        public long ContentLength { get { return Response.ContentLength; } }

        /// <summary>
        /// Gets the content type of the data being received.
        /// </summary>
        public ContentType ContentType { get { return _contentType ?? InitializeContentType(); } }

        /// <summary>
        /// Gets the URI of the Internet resource that actually responded to the request.
        /// </summary>
        public Uri ResponseUri { get { return Response.ResponseUri; } }

        /// <summary>
        /// Gets a collection of header name-value pairs associated with this request.
        /// </summary>
        public WebHeaderCollection Headers { get { return Response.Headers; } }

        /// <returns>
        /// Returns whenever the request supports headers.
        /// </returns>
        public bool SupportsHeaders { get { return Response.SupportsHeaders; } }

        /// <summary>
        /// Initializes an instance of the HttpResponseStream class.
        /// </summary>
        protected HttpResponseStream()
        {
        }

        /// <summary>
        /// Initializes the response stream from the WebRequest.
        /// </summary>
        public static HttpResponseStream InitializeResponseStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var stream = new HttpResponseStream { Response = request.GetResponse() };

            stream.RawResponseStream = stream.Response.GetResponseStream();
            if (stream.RawResponseStream == null)
                throw new Exception(Resources.Text.null_response_stream);

            return stream;
        }

        /// <summary>
        /// Returns the content of the response stream as a byte array.
        /// </summary>
        public byte[] AsByteArray()
        {
            using (var memoryStream = new MemoryStream())
            {
                DataStream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Returns the content of the response stream as a string using the specified encoding.
        /// If the encoding is null it will try to infer the encoding from the response's character set.
        /// If the encoding cannot be inferred then it assumes text data and uses ISO-8859-1 
        /// (see 3.7.1 of RFC 2616 for default charset for text subtypes).
        /// </summary>
        public string AsString(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = GetEncoding();

            using (var reader = new StreamReader(DataStream, encoding, true, 4096, true))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Releases the response and the streams.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_dataStream != null)
                    _dataStream.Dispose();

                if (RawResponseStream != null)
                    RawResponseStream.Dispose();

                if (Response != null)
                    Response.Close();
            }

            base.Dispose(disposing);
        }

        ContentType InitializeContentType()
        {
            // ReSharper disable EmptyGeneralCatchClause
            var contentType = Response.ContentType;

            try
            {
                if (!string.IsNullOrWhiteSpace(contentType))
                    return (_contentType = new ContentType(Response.ContentType));
            }
            catch
            {
            }

            return (_contentType = new ContentType());
            // ReSharper restore EmptyGeneralCatchClause
        }

        /// <summary>
        /// Tries to figure out the response stream encoding. If it cannot then CharSets.Iso88591 is returned.
        /// </summary>
        /// <returns></returns>
        protected Encoding GetEncoding()
        {
            try
            {
                var httpResponse = Response as HttpWebResponse;

                if (httpResponse != null && (!string.IsNullOrWhiteSpace(httpResponse.CharacterSet)))
                    return Encoding.GetEncoding(httpResponse.CharacterSet);

                return string.IsNullOrWhiteSpace(ContentType.CharSet)
                    ? DefaultTextEncoding
                    : Encoding.GetEncoding(ContentType.CharSet);
            }
            catch
            {
                return DefaultTextEncoding;
            }
        }

        #region Stream

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return DataStream.CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return DataStream.CanSeek; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return DataStream.CanWrite; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream can timeout. 
        /// </summary>
        public override bool CanTimeout
        {
            get { return DataStream.CanTimeout; }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length
        {
            get { return DataStream.Length; }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get { return DataStream.Position; }
            set { DataStream.Position = value; }
        }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out. 
        /// </summary>
        public override int ReadTimeout
        {
            get { return DataStream.ReadTimeout; }
            set { DataStream.ReadTimeout = value; }
        }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out. 
        /// </summary>
        public override int WriteTimeout
        {
            get { return DataStream.WriteTimeout; }
            set { DataStream.WriteTimeout = value; }
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            DataStream.Flush();
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return DataStream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        public override void SetLength(long value)
        {
            DataStream.SetLength(value);
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return DataStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            DataStream.Write(buffer, offset, count);
        }

        #endregion Stream
    }
}
