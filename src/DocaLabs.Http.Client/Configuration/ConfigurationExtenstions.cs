﻿using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines operations on configuration elements.
    /// </summary>
    public static class ConfigurationExtenstions
    {
        /// <summary>
        /// Gets credentials from the description.
        /// </summary>
        /// <returns></returns>
        static public ICredentials GetCredential(this IClientNetworkCredential credential)
        {
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

        /// <summary>
        /// Finds a certificate referenced by an instance of the class.
        /// </summary>
        /// <returns>Found certificate or null if the certificate is not found, or throws MoreThanOneMatchFoundException if there are more than one match.</returns>
        static public X509CertificateCollection Find(this IClientCertificateReference reference)
        {
            var certStore = new X509Store(reference.StoreName, reference.StoreLocation);

            try
            {
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                return certStore.Certificates.Find(reference.X509FindType, reference.FindValue, true);
            }
            finally
            {
                certStore.Close();
            }
        }

        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        static public void CopyHeadersTo(this IClientEndpoint endpoint, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (var name in endpoint.Headers.AllKeys)
                request.Headers.Add(name, endpoint.Headers[name].Value);
        }

        /// <summary>
        /// If client certificates are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        static public void CopyClientCertificatesTo(this IClientEndpoint endpoint, HttpWebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (IClientCertificateReference certRef in endpoint.ClientCertificates)
                request.ClientCertificates.AddRange(certRef.Find());
        }

        /// <summary>
        /// If the AuthenticationLevel and Credential are defined then the method copies them into the request.
        /// </summary>
        static public void CopyCredentialsTo(this IClientEndpoint endpoint, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (endpoint.AuthenticationLevel != null)
                request.AuthenticationLevel = endpoint.AuthenticationLevel.GetValueOrDefault();

            request.Credentials = endpoint.Credential.GetCredential();
        }

        /// <summary>
        /// If web proxy are defined in the endpoint configuration then the methods adds it to the request.
        /// </summary>
        static public void CopyWebProxyTo(this IClientEndpoint endpoint, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (endpoint.Proxy != null && endpoint.Proxy.Address != null)
                request.Proxy = new WebProxy(endpoint.Proxy.Address) { Credentials = endpoint.Proxy.Credential.GetCredential() };
        }

        static ICredentials GetNetworkCredential(IClientNetworkCredential credential)
        {
            return new NetworkCredential(
                string.IsNullOrWhiteSpace(credential.User) ? string.Empty : credential.User,
                string.IsNullOrWhiteSpace(credential.Password) ? string.Empty : credential.Password,
                string.IsNullOrWhiteSpace(credential.Domain) ? string.Empty : credential.Domain);
        }
    }
}