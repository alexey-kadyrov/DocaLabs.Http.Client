using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    public interface ICredentialsMapper
    {
        /// <summary>
        /// Maps the model to credentials by checking whenever any of its properties returns non null object implementing ICredentials.
        /// If more then one NetworkCredential value is detected then CredentialCache object is returned with all of then cached.
        /// In this case the UriPartial.Authority of the URL is used to add credentials to the CredentialCache and the property name is used as the authentication type.
        /// </summary>
        ICredentials Map(object client, object model, Uri url);
    }
}