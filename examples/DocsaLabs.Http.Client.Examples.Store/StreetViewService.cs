using System;
using System.IO;
using DocaLabs.Http.Client;
using DocsaLabs.Http.Client.Examples.Core;

namespace DocsaLabs.Http.Client.Examples.Store
{
    public class StreetViewService : AsyncHttpClient<StreetViewRequest, Stream>, IStreetViewService
    {
        public StreetViewService(Uri baseUrl = null, string configurationName = null)
            : base(baseUrl, configurationName)
        {
        }
    }
}