using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Wraps around the response stream.
    /// </summary>
    public abstract class HttpResponseStreamCore : Stream, IHttpResponseStream
    {
        static protected readonly Encoding DefaultTextEncoding = Encoding.GetEncoding(CharSets.Iso88591);

        Stream _dataStream;
        HttpContentType _contentType;
        Stream _rawResponseStream;

        protected WebResponse Response { get; set; }

        /// <summary>
        /// Gets or sets the raw response stream.
        /// </summary>
        protected Stream RawResponseStream { get { return _rawResponseStream ?? (_rawResponseStream = InitializeResponseStream()); } }

        /// <summary>
        /// Returns the response stream, if the content is encoded (compressed) then it will be decoded using decoder provided by ContentDecoderFactory.
        /// </summary>
        protected Stream DataStream
        {
            get
            {
                if (_dataStream != null)
                    return _dataStream;

                var contentEncoding = Response.GetContentEncoding();
                if (string.IsNullOrWhiteSpace(contentEncoding))
                    return (_dataStream = RawResponseStream);

                return (_dataStream = ContentDecoderFactory.Get(contentEncoding).GetDecompressionStream(RawResponseStream));
            }
        }

        /// <summary>
        /// Gets the content type of the data being received.
        /// </summary>
        public HttpContentType ContentType { get { return _contentType ?? InitializeContentType(); } }

        /// <summary>
        /// Gets the content length of data being received.
        /// </summary>
        public long ContentLength { get { return Response.ContentLength; } }

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
        /// Releases the response and the streams.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dataStream != null)
                    _dataStream.Dispose();

                if (RawResponseStream != null)
                    RawResponseStream.Dispose();

                if (Response != null)
                    Response.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Tries to figure out the response stream encoding. If it cannot then CharSets.Iso88591 is returned.
        /// </summary>
        /// <returns></returns>
        protected virtual Encoding GetEncoding()
        {
            try
            {
                var charSet = new HttpContentType(Response.ContentType).CharSet;

                if (!string.IsNullOrWhiteSpace(charSet))
                    return Encoding.GetEncoding(charSet);

                return string.IsNullOrWhiteSpace(ContentType.CharSet)
                    ? DefaultTextEncoding
                    : Encoding.GetEncoding(ContentType.CharSet);
            }
            catch
            {
                return DefaultTextEncoding;
            }
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
        /// Asynchronously reads the response stream and returns as a byte array.
        /// </summary>
        public async Task<byte[]> AsByteArrayAsync(CancellationToken cancellationToken)
        {
            using (var memoryStream = new MemoryStream())
            {
                await DataStream.CopyToAsync(memoryStream, 4096, cancellationToken);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Asynchronously reads the response stream and returns the content of the response stream as a string using the specified encoding.
        /// If the encoding is null it will try to infer the encoding from the response's character set.
        /// If the encoding cannot be inferred then it assumes text data and uses ISO-8859-1 
        /// (see 3.7.1 of RFC 2616 for default charset for text subtypes).
        /// </summary>
        public Task<string> AsStringAsync(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = GetEncoding();

            using (var reader = new StreamReader(DataStream, encoding, true, 4096, true))
            {
                return reader.ReadToEndAsync();
            }
        }

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
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream
        /// </summary>
        /// <returns>The unsigned byte cast to an Int32, or -1 if at the end of the stream.</returns>
        public override int ReadByte()
        {
            return DataStream.ReadByte();
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

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        public override void WriteByte(byte value)
        {
            DataStream.WriteByte(value);
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write the data into.</param>
        /// <param name="offset">The byte offset in buffer at which to begin writing data from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return DataStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The zero-based byte offset in buffer from which to begin copying bytes to the stream.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return DataStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 4096.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return DataStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return DataStream.FlushAsync(cancellationToken);
        }

        Stream InitializeResponseStream()
        {
            var stream = Response.GetResponseStream();

            if (RawResponseStream == null)
                throw new Exception(Resources.Text.null_response_stream);

            return stream;
        }

        HttpContentType InitializeContentType()
        {
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                return _contentType = new HttpContentType(Response.ContentType);
            }
            catch
            {
            }

            return _contentType = new HttpContentType(HttpContentType.Default);
            // ReSharper restore EmptyGeneralCatchClause
        }
    }
}