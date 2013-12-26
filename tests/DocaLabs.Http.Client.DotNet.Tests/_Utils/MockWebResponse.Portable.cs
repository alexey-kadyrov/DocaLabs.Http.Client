using System;
using System.IO;
using System.Net;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class MockWebResponse : WebResponse
    {
        readonly Stream _responseStream;
        WebHeaderCollection _headers;
        string _contentType;
        bool _isMutuallyAuthenticated;
        Uri _responseUri;

        public override long ContentLength { get { return _responseStream.Length; } }

        public override string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public override bool SupportsHeaders
        {
            get { return true; }
        }

        public override WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        public override bool IsMutuallyAuthenticated
        {
            get { return _isMutuallyAuthenticated; }
        }

        public override Uri ResponseUri
        {
            get { return _responseUri; }
        }

        public int TestCloseCounter { get; private set; }

        public MockWebResponse(Stream responseStream, string contentType)
        {
            _responseStream = responseStream;
            _contentType = contentType;
            _headers = new WebHeaderCollection();
        }

        public override Stream GetResponseStream()
        {
            return _responseStream;
        }

        public override void Close()
        {
            TestCloseCounter++;
        }

        public void SetIsMutuallyAuthenticated(bool value)
        {
            _isMutuallyAuthenticated = value;
        }

        public void SetResponseUri(Uri value)
        {
            _responseUri = value;
        }

        public void SetHeaders(WebHeaderCollection value)
        {
            _headers = value;
        }
    }
}
