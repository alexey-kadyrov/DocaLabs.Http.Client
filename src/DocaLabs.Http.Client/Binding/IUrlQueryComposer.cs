using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlQueryComposer
    {
        string Compose(object model, Uri baseUrl);
    }
}
