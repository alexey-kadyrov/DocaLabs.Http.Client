using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlComposer
    {
        string Compose(object model, object client, Uri baseUrl);
    }
}