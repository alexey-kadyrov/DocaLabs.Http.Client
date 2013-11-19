using System.IO;
using System.Threading.Tasks;

namespace DocsaLabs.Http.Client.Examples.Phone
{
    public interface IStreetViewService
    {
        Task<Stream> ExecuteAsync(StreetViewRequest request);
    }
}
