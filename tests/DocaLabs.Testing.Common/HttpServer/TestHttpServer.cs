using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DocaLabs.Testing.Common.HttpServer
{
    public class TestHttpServer : SimpleHttpServer
    {
        public RequestData LastRequestData { get; set; }
        public ResponseData ResponseToBeSent { get; set; }

        public TestHttpServer(IPEndPoint endPoint)
            : base(endPoint)
        {
        }

        public override void HandleGetRequest(HttpProcessor p)
        {
            Debug.WriteLine(@"request: {0}", p.HttpUrl);

            LastRequestData = new RequestData(p.HttpMethod, p.HttpUrl, p.HttpProtocolVersionstring, p.HttpHeaders);

            SendResponse(p);
        }

        public override void HandlePostRequest(HttpProcessor p, StreamReader inputData)
        {
            Debug.WriteLine(@"POST request: {0}", p.HttpUrl);

            LastRequestData = new RequestData(p.HttpMethod, p.HttpUrl, p.HttpProtocolVersionstring, p.HttpHeaders)
            {
                Data = inputData.ReadToEnd() 
            };

            SendResponse(p);
        }

        void SendResponse(HttpProcessor p)
        {
            p.OutputWriter.WriteLine("HTTP/1.0 200 OK");

            if (ResponseToBeSent != null && !string.IsNullOrWhiteSpace(ResponseToBeSent.ContentType))
                p.OutputWriter.WriteLine("Content-Type: {0}", ResponseToBeSent.ContentType);

            p.OutputWriter.WriteLine("Connection: close");
            p.OutputWriter.WriteLine("");

            if (ResponseToBeSent != null && !string.IsNullOrWhiteSpace(ResponseToBeSent.Data))
                p.OutputWriter.WriteLine(ResponseToBeSent.Data);
        }

        public class RequestData
        {
            public string HttpMethod { get; private set; }
            public string HttpUrl { get; private set; }
            public string HttpProtocolVersionstring { get; private set; }
            public Hashtable HttpHeaders { get; private set; }

            public string Data { get; set; }

            public RequestData(string httpMethod, string httpUrl, string httpProtocolVersionstring, Hashtable httpHeaders)
            {
                HttpHeaders = httpHeaders;
                HttpProtocolVersionstring = httpProtocolVersionstring;
                HttpUrl = httpUrl;
                HttpMethod = httpMethod;
            }
        }

        public class ResponseData
        {
            public string ContentType { get; set; }
            public string Data { get; set; }
        }
    }
}
