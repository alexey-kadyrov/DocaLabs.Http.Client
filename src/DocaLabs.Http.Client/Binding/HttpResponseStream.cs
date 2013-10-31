using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Wraps around the response stream.
    /// </summary>
    public class HttpResponseStream : HttpResponseStreamCore
    {
        /// <summary>
        /// Gets a value that indicates whether mutual authentication occurred.
        /// </summary>
        public bool IsMutuallyAuthenticated { get { return Response.IsMutuallyAuthenticated; } }

        HttpResponseStream()
        {
        }

        /// <summary>
        /// Initializes the response stream from the WebRequest.
        /// </summary>
        public static HttpResponseStream CreateResponseStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var stream = new HttpResponseStream { Response = request.GetResponse() };

            stream.InitializeResponseStream();

            return stream;
        }

        /// <summary>
        /// Initializes an instance of the AsyncHttpResponseStream class for the provided WebRequest instance.
        /// </summary>
        public static async Task<HttpResponseStream> CreateAsyncResponseStream(WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var stream = new HttpResponseStream();
            
            stream.Response = await request.GetResponseAsync();

            stream.InitializeResponseStream();

            return stream;
        }

        /// <summary>
        /// Begins an asynchronous read operation. Consider using ReadAsync instead.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The byte offset in buffer at which to begin writing data read from the stream.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
        /// <returns>An IAsyncResult that represents the asynchronous read, which could still be pending.</returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return DataStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Waits for the pending asynchronous read to complete. Consider using ReadAsync instead.
        /// </summary>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
        /// <returns>The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.</returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return DataStream.EndRead(asyncResult);
        }

        /// <summary>
        /// Begins an asynchronous write operation. Consider using WriteAsync instead.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The byte offset in buffer from which to begin writing.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        /// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
        /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
        /// <returns>An IAsyncResult that represents the asynchronous write, which could still be pending.</returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return DataStream.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Ends an asynchronous write operation. Consider using WriteAsync instead.
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            DataStream.EndWrite(asyncResult);
        }
    }
}
