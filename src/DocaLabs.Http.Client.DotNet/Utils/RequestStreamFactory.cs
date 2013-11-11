using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Utils
{
    public class RequestStreamFactory : IRequestStreamFactory
    {
        public Stream Get(WebRequest request)
        {
            return request.GetRequestStream();
        }

        public Task<Stream> GetAsync(WebRequest request)
        {
            return request.GetRequestStreamAsync();
        }
    }
}
