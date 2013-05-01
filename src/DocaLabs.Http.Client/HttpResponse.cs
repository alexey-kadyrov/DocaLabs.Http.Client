using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// A wrapper around WebResponse instance.
    /// </summary>
    public class HttpResponse : IDisposable
    {
        ContentType _contentType;

        WebResponse Response { get; set; }

        Stream RawResponseStream { get; set; }

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
        /// Returns <see cref="T:System.Boolean"/>.
        /// </returns>
        public bool SupportsHeaders { get { return Response.SupportsHeaders; } }

        /// <summary>
        /// Initializes an instance of the HttpResponse class for the provided WebRequest instance.
        /// </summary>
        public HttpResponse(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            Response = request.GetResponse();

            RawResponseStream = Response.GetResponseStream();
            if (RawResponseStream == null)
                throw new HttpClientException(Resources.Text.null_response_stream);
        }

        /// <summary>
        /// Returns the content of the response stream as a byte array.
        /// </summary>
        public byte[] AsByteArray()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var stream = GetDataStream())
                {
                    stream.CopyTo(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Returns the content of the response stream as a string.
        /// If the encoding cannot be inferred from the response then UTF-8 is assumed.
        /// </summary>
        public string AsString()
        {
            // stream is disposed by the reader
            using (var reader = new StreamReader(GetDataStream(), TryGetEncoding()))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Tries to infer the response encoding for WebResponse or returns UTF-8 otherwise.
        /// </summary>
        /// <returns></returns>
        public Encoding TryGetEncoding()
        {
            try
            {
                var httpResponse = Response as HttpWebResponse;

                if (httpResponse != null && (!string.IsNullOrWhiteSpace(httpResponse.CharacterSet)))
                    return Encoding.GetEncoding(httpResponse.CharacterSet);

                return ContentType.CharSet == null 
                    ? Encoding.UTF8 
                    : Encoding.GetEncoding(ContentType.CharSet);
            }
            catch
            {
                return Encoding.UTF8;
            }
        }

        /// <summary>
        /// Returns the response stream, if the content is encoded (compressed) then it will be decoded using decoder provided by ContentDecoderFactory.
        /// </summary>
        /// <returns>RawResponseStream or a stream containing decoded content.</returns>
        public Stream GetDataStream()
        {
            var httpResponse = Response as HttpWebResponse;
            if (httpResponse == null || string.IsNullOrWhiteSpace(httpResponse.ContentEncoding))
                return RawResponseStream;

            return ContentDecoderFactory.Get(httpResponse.ContentEncoding).GetDecompressionStream(RawResponseStream);
        }

        /// <summary>
        /// Releases the response and the stream.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the response and the stream.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;

            if(RawResponseStream != null)
                RawResponseStream.Dispose();

            if(Response != null)
                Response.Close();
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
    }
}
