using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface ICredentialsMapper
    {
        ICredentials Map(object model, Uri url);
    }
}
