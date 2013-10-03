using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Adds asynchronous support on the top of the HttpResponseStream.
    /// </summary>
    public class AsyncHttpResponseStream : HttpResponseStream
    {
        CancellationToken _cancellationToken;

        /// <summary>
        /// Initializes an instance of the AsyncHttpResponseStream class.
        /// </summary>
        protected AsyncHttpResponseStream()
        {
        }

        /// <summary>
        /// Initializes an instance of the AsyncHttpResponseStream class for the provided WebRequest instance.
        /// </summary>
        public static async Task<AsyncHttpResponseStream> InitializeAsyncHttpResponseStream(WebRequest request, CancellationToken cancellationToken) 
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var stream = new AsyncHttpResponseStream
            {
                _cancellationToken = cancellationToken,
                Response = await request.GetResponseAsync()
            };

            stream.RawResponseStream = stream.Response.GetResponseStream();
            if (stream.RawResponseStream == null)
                throw new Exception(Resources.Text.null_response_stream);

            return stream;
        }

        /// <summary>
        /// Asynchronously reads the response stream and returns as a byte array.
        /// </summary>
        public async Task<byte[]> AsByteArrayAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await DataStream.CopyToAsync(memoryStream, 4096, _cancellationToken);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Returns the content of the response stream as a string using the specified encoding.
        /// If the encoding is null it will try to infer the encoding from the response's character set.
        /// If the encoding cannot be inferred then it assumes text data and uses ISO-8859-1 
        /// (see 3.7.1 of RFC 2616 for default charset for text subtypes).
        /// </summary>
        public async Task<string> AsStringAsync(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = GetEncoding();

            using (var reader = new StreamReader(DataStream, encoding, true, 4096, true))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public new async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await DataStream.ReadAsync(buffer, offset, count, _cancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public new async Task Write(byte[] buffer, int offset, int count)
        {
            await DataStream.WriteAsync(buffer, offset, count, _cancellationToken);
        }
    }
}
