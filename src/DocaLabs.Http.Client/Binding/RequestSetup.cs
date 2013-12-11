using System;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines additional operations on web request.
    /// </summary>
    public class RequestSetup : IRequestSetup
    {
        readonly static IRequestStreamFactory RequestStreamFactory = PlatformAdapter.Resolve<IRequestStreamFactory>();

        /// <summary>
        /// Writes 0 bytes to the request stream due that Store and Windows Phone API doesn't expose ContentLength property (despite that is accessible through reflection and it's public)
        /// </summary>
        public virtual void SetContentLengthToZeroIfBodyIsRequired(WebRequest request, BindingContext context)
        {
            if (!IsBodyRequired(request)) 
                return;

            using (var stream = RequestStreamFactory.Get(context, request))
            {
                stream.Write(new byte[0], 0, 0 );
            }
        }

        /// <summary>
        /// Writes 0 bytes to the request stream due that Store and Windows Phone API doesn't expose ContentLength property (despite that is accessible through reflection and it's public)
        /// </summary>
        public virtual async Task SetContentLengthToZeroIfBodyIsRequiredAsync(WebRequest request, AsyncBindingContext context)
        {
            if (!IsBodyRequired(request))
                return;

            using (var stream = await RequestStreamFactory.GetAsync(context, request))
            {
                stream.Write(new byte[0], 0, 0);
            }
        }

        /// <summary>
        /// Returns true if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public virtual bool IsBodyRequired(WebRequest request)
        {
            return
                (
                    request is HttpWebRequest
                ) &&
                (
                    string.Compare(request.Method, "POST", StringComparison.OrdinalIgnoreCase) == 0 ||
                    string.Compare(request.Method, "PUT", StringComparison.OrdinalIgnoreCase) == 0
                );
        }

        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        public virtual void CopyHeadersFrom(WebRequest request, IRequestBinder binder, BindingContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(context.Configuration != null)
                CopeHeadersFromConfiguration(context.Configuration, request);

            if(context.Model != null)
                MapHeadersFromModel(binder, context, request);
        }

        /// <summary>
        /// Does nothing as it's not supported on all platforms.
        /// </summary>
        public virtual void CopyClientCertificatesFrom(WebRequest request, IClientEndpoint endpoint)
        {
        }

        /// <summary>
        /// If the AuthenticationLevel and Credential are defined then the method copies them into the request.
        /// </summary>
        public virtual void CopyCredentialsFrom(WebRequest request, IRequestBinder binder, BindingContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (context.Configuration != null)
            {
                request.Credentials = GetCredentialsFromModel(binder, context) ?? GetCredential(context.Configuration.Credential);
            }
            else
            {
                request.Credentials = GetCredentialsFromModel(binder, context);
            }
        }

        /// <summary>
        /// Does nothing as it's not supported on all platforms.
        /// </summary>
        public virtual void CopyWebProxyFrom(WebRequest request, IClientEndpoint endpoint)
        {
        }

        protected ICredentials GetCredentialsFromModel(IRequestBinder binder, BindingContext context)
        {
            return context.Model == null
                ? null
                : binder.GetCredentials(context);
        }

        protected ICredentials GetNetworkCredential(IClientNetworkCredential credential)
        {
            return new NetworkCredential(
                string.IsNullOrWhiteSpace(credential.User) ? string.Empty : credential.User,
                string.IsNullOrWhiteSpace(credential.Password) ? string.Empty : credential.Password,
                string.IsNullOrWhiteSpace(credential.Domain) ? string.Empty : credential.Domain);
        }

        ICredentials GetCredential(IClientNetworkCredential credential)
        {
            if (credential == null)
                throw new ArgumentNullException("credential");

            switch (credential.CredentialType)
            {
                case CredentialType.DefaultCredentials:
                    throw new PlatformNotSupportedException(Resources.Text.default_credentials_are_not_supported);

                case CredentialType.DefaultNetworkCredentials:
                    throw new PlatformNotSupportedException(Resources.Text.default_network_credentials_are_not_supported1);

                case CredentialType.NetworkCredential:
                    return GetNetworkCredential(credential);

                default:
                    return null;
            }
        }

        static void CopeHeadersFromConfiguration(IClientEndpoint endpoint, WebRequest request)
        {
            foreach (var header in endpoint.Headers)
                request.Headers[header.Name] = header.Value;
        }

        static void MapHeadersFromModel(IRequestBinder binder, BindingContext context, WebRequest request)
        {
            var headers = binder.GetHeaders(context);
            if (headers == null)
                return;

            foreach (var key in headers.AllKeys)
                request.Headers[key] = headers[key];
        }
    }
}
