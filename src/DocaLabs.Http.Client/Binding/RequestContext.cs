using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public class RequestContext
    {
        public object HttpClient { get; private set; }
        public object QueryModel { get; private set; }
        public WebRequest Request { get; set; }
    }
}
