using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Utils
{
    public interface IRequestStreamFactory
    {
        Stream Get(WebRequest request);
        Task<Stream> GetAsync(WebRequest request);
    }
}
