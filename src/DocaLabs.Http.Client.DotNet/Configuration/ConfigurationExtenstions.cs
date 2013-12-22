using System;
using System.ComponentModel;
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

                return certStore.Certificates.Find(ToX509FindType(reference.X509FindType), reference.FindValue, true);
            }
            finally
            {
                certStore.Close();
            }
        }

        public static StoreLocation ToStoreLocation(this CertificateStoreLocation value)
        {
            switch (value)
            {
                case CertificateStoreLocation.CurrentUser:
                    return StoreLocation.CurrentUser;
                case CertificateStoreLocation.LocalMachine:
                    return StoreLocation.LocalMachine;
                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(CertificateStoreLocation));
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

        public static X509FindType ToX509FindType(this CertificateX509FindType value)
        {
            switch (value)
            {
                case CertificateX509FindType.FindByThumbprint:
                    return X509FindType.FindByThumbprint;
                case CertificateX509FindType.FindBySubjectName:
                    return X509FindType.FindBySubjectName;
                case CertificateX509FindType.FindBySubjectDistinguishedName:
                    return X509FindType.FindBySubjectDistinguishedName;
                case CertificateX509FindType.FindByIssuerName:
                    return X509FindType.FindByIssuerName;
                case CertificateX509FindType.FindByIssuerDistinguishedName:
                    return X509FindType.FindByIssuerDistinguishedName;
                case CertificateX509FindType.FindBySerialNumber:
                    return X509FindType.FindBySerialNumber;
                case CertificateX509FindType.FindByTimeValid:
                    return X509FindType.FindByTimeValid;
                case CertificateX509FindType.FindByTimeNotYetValid:
                    return X509FindType.FindByTimeNotYetValid;
                case CertificateX509FindType.FindByTimeExpired:
                    return X509FindType.FindByTimeExpired;
                case CertificateX509FindType.FindByTemplateName:
                    return X509FindType.FindByTemplateName;
                case CertificateX509FindType.FindByApplicationPolicy:
                    return X509FindType.FindByApplicationPolicy;
                case CertificateX509FindType.FindByCertificatePolicy:
                    return X509FindType.FindByCertificatePolicy;
                case CertificateX509FindType.FindByExtension:
                    return X509FindType.FindByExtension;
                case CertificateX509FindType.FindByKeyUsage:
                    return X509FindType.FindByKeyUsage;
                case CertificateX509FindType.FindBySubjectKeyIdentifier:
                    return X509FindType.FindBySubjectKeyIdentifier;
                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(CertificateX509FindType));
            }
        }
    }
}
