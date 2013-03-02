using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface IRequestStreamWriter
    {
        void Write(object model, object client, WebRequest request);
    }
}
