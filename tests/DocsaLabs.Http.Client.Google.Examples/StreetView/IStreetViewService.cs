using System.IO;

namespace DocsaLabs.Http.Client.Google.Examples.StreetView
{
    public interface IStreetViewService
    {
        Stream Fetch(StreetViewRequest request);
    }
}
