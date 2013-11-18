using System.IO;
using System.Threading.Tasks;
using DocsaLabs.Http.Client.Examples.Core;

namespace DocsaLabs.Http.Client.Examples.DotNet
{
    public interface IStreetViewService
    {
        Task<Stream> FetchAsync(StreetViewRequest request);
    }
}
