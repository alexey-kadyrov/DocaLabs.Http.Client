using System;

namespace DocaLabs.Http.Client.Binding
{
    public class RequestContext
    {
        public object HttpClient { get; private set; }
        public object QueryModel { get; private set; }
        public Uri BaseUrl { get; set; }
    }
}
