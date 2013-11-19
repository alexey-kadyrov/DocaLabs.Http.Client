using System.IO;
using System.Threading.Tasks;

namespace DocsaLabs.Http.Client.Examples.DotNet
{
    public interface IStreetViewService
    {
        Task<Stream> FetchAsync(StreetViewRequest request);
    }
}
