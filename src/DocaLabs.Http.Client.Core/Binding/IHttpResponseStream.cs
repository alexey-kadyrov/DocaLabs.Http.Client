using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public interface IHttpResponseStream : IDisposable
    {
        WebResponse Response { get; }

        /// <summary>
        /// Gets the content type of the data being received.
        /// </summary>
        HttpContentType ContentType { get; }

        /// <summary>
        /// Gets the content length of data being received.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the URI of the Internet resource that actually responded to the request.
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets a collection of header name-value pairs associated with this request.
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <returns>
        /// Returns whenever the request supports headers.
        /// </returns>
        bool SupportsHeaders { get; }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        bool CanSeek { get; }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// Gets a value indicating whether the current stream can timeout. 
        /// </summary>
        bool CanTimeout { get; }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out. 
        /// </summary>
        int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out. 
        /// </summary>
        int WriteTimeout { get; set; }

        /// <summary>
        /// Returns the content of the response stream as a byte array.
        /// </summary>
        byte[] AsByteArray();

        /// <summary>
        /// Returns the content of the response stream as a string using the specified encoding.
        /// If the encoding is null it will try to infer the encoding from the response's character set.
        /// If the encoding cannot be inferred then it assumes text data and uses ISO-8859-1 
        /// (see 3.7.1 of RFC 2616 for default charset for text subtypes).
        /// </summary>
        string AsString(Encoding encoding = null);

        /// <summary>
        /// Asynchronously reads the response stream and returns as a byte array.
        /// </summary>
        Task<byte[]> AsByteArrayAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously reads the response stream and returns the content of the response stream as a string using the specified encoding.
        /// If the encoding is null it will try to infer the encoding from the response's character set.
        /// If the encoding cannot be inferred then it assumes text data and uses ISO-8859-1 
        /// (see 3.7.1 of RFC 2616 for default charset for text subtypes).
        /// </summary>
        Task<string> AsStringAsync(Encoding encoding = null);

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        void Flush();

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        long Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        void SetLength(long value);

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream
        /// </summary>
        /// <returns>The unsigned byte cast to an Int32, or -1 if at the end of the stream.</returns>
        int ReadByte();

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        void WriteByte(byte value);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write the data into.</param>
        /// <param name="offset">The byte offset in buffer at which to begin writing data from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
        Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The zero-based byte offset in buffer from which to begin copying bytes to the stream.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.
        /// </summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 4096.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>A task that represents the asynchronous flush operation.</returns>
        Task FlushAsync(CancellationToken cancellationToken);

        void CopyTo(Stream destination);
        void CopyTo(Stream destination, int bufferSize);
        Task CopyToAsync(Stream destination);
        Task CopyToAsync(Stream destination, int bufferSize);
        Task FlushAsync();
        Task<int> ReadAsync(byte[] buffer, int offset, int count);
        Task WriteAsync(byte[] buffer, int offset, int count);
    }
}