using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines how to find a certificate. 
    /// </summary>
    public class ClientCertificateReferenceElement : ConfigurationElement, IClientCertificateReference
    {
        const string StoreNameProperty = "storeName";
        const string StoreLocationProperty = "storeLocation";
        const string X509FindTypeProperty = "x509FindType";
        const string FindValueProperty = "findValue";

        /// <summary>
        /// Gets or sets the name of the X.509 certificate store to open.
        /// </summary>
        [ConfigurationProperty(StoreNameProperty, IsKey = true, DefaultValue = CertificateStoreName.My)]
        public CertificateStoreName StoreName
        {
            get { return (CertificateStoreName)this[StoreNameProperty]; }
            set { this[StoreNameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the location of the certificate store the client can use to validate the server’s certificate.
        /// </summary>
        [ConfigurationProperty(StoreLocationProperty, IsKey = true, DefaultValue = CertificateStoreLocation.LocalMachine)]
        public CertificateStoreLocation StoreLocation
        {
            get { return (CertificateStoreLocation)this[StoreLocationProperty]; }
            set { this[StoreLocationProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the type of X.509 search to be executed.
        /// </summary>
        [ConfigurationProperty(X509FindTypeProperty, IsKey = true, DefaultValue = CertificateX509FindType.FindBySubjectDistinguishedName)]
        public CertificateX509FindType X509FindType
        {
            get { return (CertificateX509FindType)this[X509FindTypeProperty]; }
            set { this[X509FindTypeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets a string that specifies the value to search for in the X.509 certificate store.
        /// </summary>
        [ConfigurationProperty(FindValueProperty, IsKey = true, DefaultValue = "")]
        public string FindValue
        {
            get { return (string)this[FindValueProperty]; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = string.Empty;

                this[FindValueProperty] = value;
            }
        }
    }
}
