using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface IRequestWriter
    {
        void Write(object model, object client, WebRequest request);
    }
}
