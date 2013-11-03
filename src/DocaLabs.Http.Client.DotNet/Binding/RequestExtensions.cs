using System;
using System.Net;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines additional operations on web request.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Sets the ContentLength to zero if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public static void SetContentLengthToZeroIfBodyIsRequired(this WebRequest request)
        {
            if (IsBodyRequired(request))
                request.ContentLength = 0;
        }

        /// <summary>
        /// Returns true if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        public static bool IsBodyRequired(this WebRequest request)
        {
            return
                (
                    request is HttpWebRequest || request is FileWebRequest
                ) &&
                (
                    string.Compare(request.Method, WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                    string.Compare(request.Method, WebRequestMethods.Http.Put, StringComparison.InvariantCultureIgnoreCase) == 0
                );
        }

        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        static public void CopyHeadersFrom(this WebRequest request, IRequestBinder binder, BindingContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(context.Configuration != null)
                CopeHeadersFromConfiguration(context.Configuration, request);

            if(context.Model != null)
                MapHeadersFromModel(binder, context, request);
        }

        /// <summary>
        /// If client certificates are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        static public void CopyClientCertificatesFrom(this WebRequest request, IClientEndpoint endpoint)
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
        static public void CopyCredentialsFrom(this WebRequest request, IRequestBinder binder, BindingContext context)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (context.Configuration != null)
            {
                if (context.Configuration.AuthenticationLevel != null)
                    request.AuthenticationLevel = context.Configuration.AuthenticationLevel.GetValueOrDefault();

                request.Credentials = GetCredentialsFromModel(binder, context) ?? context.Configuration.Credential.GetCredential();
            }
            else
            {
                request.Credentials = GetCredentialsFromModel(binder, context);
            }
        }

        /// <summary>
        /// If web proxy are defined in the endpoint configuration then the methods adds it to the request.
        /// </summary>
        static public void CopyWebProxyFrom(this WebRequest request, IClientEndpoint endpoint)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(endpoint == null)
                return;

            if (endpoint.Proxy != null && endpoint.Proxy.Address != null)
                request.Proxy = new WebProxy(endpoint.Proxy.Address) { Credentials = endpoint.Proxy.Credential.GetCredential() };
        }

        static void CopeHeadersFromConfiguration(IClientEndpoint endpoint, WebRequest request)
        {
            foreach (var name in endpoint.Headers.AllKeys)
                request.Headers.Add(name, endpoint.Headers[name].Value);
        }

        static void MapHeadersFromModel(IRequestBinder binder, BindingContext context, WebRequest request)
        {
            var headers = binder.GetHeaders(context);
            if (headers != null)
                request.Headers.Add(headers);
        }

        static ICredentials GetCredentialsFromModel(IRequestBinder binder, BindingContext context)
        {
            return context.Model == null
                ? null
                : binder.GetCredentials(context);
        }
    }
}
