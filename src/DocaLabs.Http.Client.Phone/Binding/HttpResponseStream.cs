using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Wraps around the response stream.
    /// </summary>
    public class HttpResponseStream : HttpResponseStreamCore
    {
        ContentType _contentType;

        internal WebResponse Response { get; set; }

        /// <summary>
        /// Gets or sets the raw response stream.
        /// </summary>
        Stream RawResponseStream { get; set; }

        Stream _dataStream;

        /// <summary>
        /// Returns the response stream, if the content is encoded (compressed) then it will be decoded using decoder provided by ContentDecoderFactory.
        /// </summary>
        protected override Stream DataStream
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
        /// Gets the content type of the data being received.
        /// </summary>
        public ContentType ContentType { get { return _contentType ?? InitializeContentType(); } }

        HttpResponseStream()
        {
        }

        /// <summary>
        /// Initializes the response stream from the WebRequest.
        /// </summary>
        public HttpResponseStream CreateResponseStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var stream = new HttpResponseStream
            {
                Response = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null).Result
            };

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

            stream.Response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);

            stream.InitializeResponseStream();

            return stream;
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
        protected override Encoding GetEncoding()
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

        void InitializeResponseStream()
        {
            RawResponseStream = Response.GetResponseStream();
            if (RawResponseStream == null)
                throw new Exception(Resources.Text.null_response_stream);
        }
    }
}
