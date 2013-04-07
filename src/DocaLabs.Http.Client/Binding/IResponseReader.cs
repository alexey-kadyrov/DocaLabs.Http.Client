using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IResponseReader
    {
        object Read(HttpResponse response, Type resultType);
    }
}
