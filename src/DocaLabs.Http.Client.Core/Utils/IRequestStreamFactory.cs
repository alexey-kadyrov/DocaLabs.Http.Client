using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Utils
{
    public interface IRequestStreamFactory
    {
        Stream Get(BindingContext context, WebRequest request);
        Task<Stream> GetAsync(AsyncBindingContext context, WebRequest request);
    }
}
