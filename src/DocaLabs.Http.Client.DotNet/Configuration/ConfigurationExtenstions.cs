﻿using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines additional operations on configuration elements.
    /// </summary>
    public static class ConfigurationExtenstions
    {
        /// <summary>
        /// Gets credentials from the description.
        /// </summary>
        /// <returns></returns>
        static public ICredentials GetCredential(this IClientNetworkCredential credential)
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

        /// <summary>
        /// Finds a certificate referenced by an instance of the class.
        /// </summary>
        /// <returns>Found certificate or null if the certificate is not found, or throws MoreThanOneMatchFoundException if there are more than one match.</returns>
        static public X509CertificateCollection Find(this IClientCertificateReference reference)
        {
            if(reference == null)
                throw new ArgumentNullException("reference");

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

        static ICredentials GetNetworkCredential(IClientNetworkCredential credential)
        {
            return new NetworkCredential(
                string.IsNullOrWhiteSpace(credential.User) ? string.Empty : credential.User,
                string.IsNullOrWhiteSpace(credential.Password) ? string.Empty : credential.Password,
                string.IsNullOrWhiteSpace(credential.Domain) ? string.Empty : credential.Domain);
        }
    }
}