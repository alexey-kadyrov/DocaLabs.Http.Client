﻿using System;
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

        internal WebResponse Response { get; private set; }

        Stream RawResponseStream { get; set; }

        Stream _dataStream;

        /// <summary>
        /// Returns the response stream, if the content is encoded (compressed) then it will be decoded using decoder provided by ContentDecoderFactory.
        /// </summary>
        Stream DataStream
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
        /// Initializes an instance of the HttpResponse class for the provided WebRequest instance.
        /// </summary>
        public HttpResponseStream(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            Response = request.GetResponse();

            RawResponseStream = Response.GetResponseStream();
            if (RawResponseStream == null)
                throw new Exception(Resources.Text.null_response_stream);
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

            using (var reader = new StreamReader(DataStream, encoding, true, 2048, true))
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

        Encoding GetEncoding()
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

        public override bool CanRead
        {
            get { return DataStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return DataStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return DataStream.CanWrite; }
        }

        public override bool CanTimeout
        {
            get { return DataStream.CanTimeout; }
        }

        public override long Length
        {
            get { return DataStream.Length; }
        }

        public override long Position
        {
            get { return DataStream.Position; }
            set { DataStream.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return DataStream.ReadTimeout; }
            set { DataStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return DataStream.WriteTimeout; }
            set { DataStream.WriteTimeout = value; }
        }

        public override void Flush()
        {
            DataStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return DataStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            DataStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return DataStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            DataStream.Write(buffer, offset, count);
        }

        #endregion Stream
    }
}
