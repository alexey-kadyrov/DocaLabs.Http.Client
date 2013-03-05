using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlPathComposer
    {
        string Compose(object model, Uri baseUrl);
    }
}
