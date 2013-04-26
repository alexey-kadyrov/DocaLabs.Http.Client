using System;
using System.Net;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines additional operations on web request.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        static public void CopyHeadersFrom(this WebRequest request, IClientEndpoint endpoint, IModelBinder binder, object httpClient, object model)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if(endpoint != null)
                CopeHeadersFromConfiguration(endpoint, request);

            if(model != null)
                MapHeadersFromModel(binder, httpClient, model, request);
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
        static public void CopyCredentialsFrom(this WebRequest request, IClientEndpoint endpoint, IModelBinder binder, object httpClient, object model)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (endpoint != null)
            {
                if (endpoint.AuthenticationLevel != null)
                    request.AuthenticationLevel = endpoint.AuthenticationLevel.GetValueOrDefault();

                request.Credentials = GetCredentialsFromModel(binder, httpClient, model, request) ?? endpoint.Credential.GetCredential();
            }
            else
            {
                request.Credentials = GetCredentialsFromModel(binder, httpClient, model, request);
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

        static void MapHeadersFromModel(IModelBinder binder, object httpClient, object model, WebRequest request)
        {
            if (model == null)
                return;

            var headers = binder.GetHeaders(httpClient, model);
            if (headers != null)
                request.Headers.Add(headers);
        }

        static ICredentials GetCredentialsFromModel(IModelBinder binder, object httpClient, object model, WebRequest request)
        {
            return model == null
                ? null
                : binder.GetCredentials(httpClient, model, request.RequestUri);
        }
    }
}
