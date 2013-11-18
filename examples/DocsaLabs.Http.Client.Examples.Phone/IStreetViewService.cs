using System.IO;
using System.Threading.Tasks;
using DocsaLabs.Http.Client.Examples.Core;

namespace DocsaLabs.Http.Client.Examples.Phone
{
    public interface IStreetViewService
    {
        Task<Stream> ExecuteAsync(StreetViewRequest request);
    }
}
