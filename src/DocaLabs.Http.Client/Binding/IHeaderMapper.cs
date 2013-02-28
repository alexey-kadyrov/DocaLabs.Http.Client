using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface IHeaderMapper
    {
        WebHeaderCollection Map(object model);
    }
}
