using System;
using System.Net;
using System.Net.Security;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines additional operations on web request.
    /// </summary>
    public class RequestSetupOverride : RequestSetup
    {
        /// <summary>
        /// Sets the ContentLength to zero if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public override void SetContentLengthToZeroIfBodyIsRequired(WebRequest request)
        {
            if (IsBodyRequired(request))
                request.ContentLength = 0;
        }

        /// <summary>
        /// Returns true if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public override bool IsBodyRequired(WebRequest request)
        {
            return
                (
                    request is HttpWebRequest || request is FileWebRequest
                ) &&
                (
                    string.Compare(request.Method, "POST", StringComparison.OrdinalIgnoreCase) == 0 ||
                    string.Compare(request.Method, "PUT", StringComparison.OrdinalIgnoreCase) == 0
                );
        }

        /// <summary>
        /// If client certificates are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        public override void CopyClientCertificatesFrom(WebRequest request, IClientEndpoint endpoint)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(endpoint == null)
                return;

            var httpRequest = request as HttpWebRequest;
            if (httpRequest == null)
                return;

            foreach (IClientCertificateReference certRef in endpoint.ClientCertificates)
                httpRequest.ClientCertificates.AddRange(certRef.Find());
        }

        /// <summary>
        /// If the AuthenticationLevel and Credential are defined then the method copies them into the request.
        /// </summary>
        public override void CopyCredentialsFrom(WebRequest request, IRequestBinder binder, BindingContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (context.Configuration != null)
            {
                if (context.Configuration.AuthenticationLevel != RequestAuthenticationLevel.Undefined)
                    request.AuthenticationLevel = ToAuthenticationLevel(context.Configuration.AuthenticationLevel);

                request.Credentials = GetCredentialsFromModel(binder, context) ?? GetCredential(context.Configuration.Credential);
            }
            else
            {
                request.Credentials = GetCredentialsFromModel(binder, context);
            }
        }

        /// <summary>
        /// If web proxy are defined in the endpoint configuration then the methods adds it to the request.
        /// </summary>
        public override void CopyWebProxyFrom(WebRequest request, IClientEndpoint endpoint)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(endpoint == null)
                return;

            if (endpoint.Proxy != null && endpoint.Proxy.Address != null)
                request.Proxy = new WebProxy(endpoint.Proxy.Address) { Credentials = GetCredential(endpoint.Proxy.Credential) };
        }

        static AuthenticationLevel ToAuthenticationLevel(RequestAuthenticationLevel level)
        {
            switch (level)
            {
                case RequestAuthenticationLevel.MutualAuthRequested:
                    return AuthenticationLevel.MutualAuthRequested;
                case RequestAuthenticationLevel.MutualAuthRequired:
                    return AuthenticationLevel.MutualAuthRequired;
                default:
                    return AuthenticationLevel.None;
            }
        }

        ICredentials GetCredential(IClientNetworkCredential credential)
        {
            if (credential == null)
                throw new ArgumentNullException("credential");

            switch (credential.CredentialType)
            {
                case CredentialType.DefaultCredentials:
                    return CredentialCache.DefaultCredentials;

                case CredentialType.DefaultNetworkCredentials:
                    return CredentialCache.DefaultNetworkCredentials;

                case CredentialType.NetworkCredential:
                    return GetNetworkCredential(credential);

                default:
                    return null;
            }
        }
    }
}
