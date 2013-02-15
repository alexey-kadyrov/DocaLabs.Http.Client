using System.IO;

namespace DocaLabs.Testing.Common.HttpServer
{
    public interface IHttpRequestHandler
    {
        void HandleGetRequest(HttpProcessor p);
        void HandlePostRequest(HttpProcessor p, StreamReader inputData);
    }
}