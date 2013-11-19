using System.IO;
using DocaLabs.Http.Client;

namespace DocsaLabs.Http.Client.Examples.Phone
{
    public class StreetViewService : AsyncHttpClient<StreetViewRequest, Stream>, IStreetViewService
    {
    }
}