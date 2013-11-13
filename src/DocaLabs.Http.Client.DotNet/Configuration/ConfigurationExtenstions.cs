using System;
using System.ComponentModel;
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
        /// Finds a certificate referenced by an instance of the class.
        /// </summary>
        /// <returns>Found certificate or null if the certificate is not found, or throws MoreThanOneMatchFoundException if there are more than one match.</returns>
        static public X509CertificateCollection Find(this IClientCertificateReference reference)
        {
            if(reference == null)
                throw new ArgumentNullException("reference");

            var certStore = new X509Store(reference.StoreName.ToStoreName(), reference.StoreLocation.ToStoreLocation());

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

        static public StoreLocation ToStoreLocation(this CertificateStoreLocation value)
        {
            switch (value)
            {
                case CertificateStoreLocation.CurrentUser:
                    return StoreLocation.CurrentUser;
                case CertificateStoreLocation.LocalMachine:
                    return StoreLocation.LocalMachine;
                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(StoreLocation));
            }
        }

        public static StoreName ToStoreName(this CertificateStoreName value)
        {
            switch (value)
            {
                case CertificateStoreName.AddressBook:
                    return StoreName.AddressBook;
                case CertificateStoreName.AuthRoot:
                    return StoreName.AuthRoot;
                case CertificateStoreName.CertificateAuthority:
                    return StoreName.CertificateAuthority;
                case CertificateStoreName.Disallowed:
                    return StoreName.Disallowed;
                case CertificateStoreName.My:
                    return StoreName.My;
                case CertificateStoreName.Root:
                    return StoreName.Root;
                case CertificateStoreName.TrustedPeople:
                    return StoreName.TrustedPeople;
                case CertificateStoreName.TrustedPublisher:
                    return StoreName.TrustedPublisher;
                default:
                    throw new InvalidEnumArgumentException("value", (int) value, typeof (CertificateStoreName));
            }
        }
    }
}
