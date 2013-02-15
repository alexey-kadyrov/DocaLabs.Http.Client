// offered to the public domain for any use with no restriction
// and also with no warranty of any kind, please enjoy. - David Jeske.
// http://www.codeproject.com/Articles/137979/Simple-HTTP-Server-in-C
// simple HTTP explanation
// http://www.jmarshall.com/easy/http/

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace DocaLabs.Testing.Common.HttpServer
{
    public abstract class SimpleHttpServer : TcpConnectionListener, IHttpRequestHandler
    {
        protected SimpleHttpServer(IPEndPoint endPoint)
            : base(endPoint)
        {
        }

        protected override void ProcessConnection(TcpClient socket)
        {
            using(var processor = new HttpProcessor(socket, this))
            {
                processor.Process();
            }
        }

        public abstract void HandleGetRequest(HttpProcessor p);
        public abstract void HandlePostRequest(HttpProcessor p, StreamReader inputData);
    }
}



