using System.IO;
using System.Threading.Tasks;

namespace DocsaLabs.Http.Client.Examples.Store
{
    public interface IStreetViewService
    {
        Task<Stream> ExecuteAsync(StreetViewRequest request);
    }
}
