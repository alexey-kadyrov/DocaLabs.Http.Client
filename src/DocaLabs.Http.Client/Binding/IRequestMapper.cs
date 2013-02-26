using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface IRequestMapper
    {
        void Map(RequestContext context);
    }
}
