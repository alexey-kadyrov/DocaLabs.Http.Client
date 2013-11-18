using System.IO;
using DocaLabs.Http.Client;
using DocsaLabs.Http.Client.Examples.Core;

namespace DocsaLabs.Http.Client.Examples.Phone
{
    public class StreetViewService : AsyncHttpClient<StreetViewRequest, Stream>, IStreetViewService
    {
    }
}