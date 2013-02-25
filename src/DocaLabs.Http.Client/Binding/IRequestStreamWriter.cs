using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface IRequestStreamWriter
    {
        void Write(RequestContext context, WebRequest request);
    }
}
