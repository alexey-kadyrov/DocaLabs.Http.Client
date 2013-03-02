using System.Configuration;
using System.Security.Cryptography.X509Certificates;

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
        [ConfigurationProperty(StoreNameProperty, IsKey = true, DefaultValue = StoreName.My)]
        public StoreName StoreName
        {
            get { return StoreNameElement; }
            set { StoreNameElement = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the location of the certificate store the client can use to validate the server’s certificate.
        /// </summary>
        [ConfigurationProperty(StoreLocationProperty, IsKey = true, DefaultValue = StoreLocation.LocalMachine)]
        public StoreLocation StoreLocation
        {
            get { return StoreLocationElement; }
            set { StoreLocationElement = value; }
        }

        /// <summary>
        /// Gets or sets the type of X.509 search to be executed.
        /// </summary>
        [ConfigurationProperty(X509FindTypeProperty, IsKey = true, DefaultValue = X509FindType.FindBySubjectDistinguishedName)]
        public X509FindType X509FindType
        {
            get { return X509FindTypeElement; }
            set { X509FindTypeElement = value; }
        }

        /// <summary>
        /// Gets or sets a string that specifies the value to search for in the X.509 certificate store.
        /// </summary>
        [ConfigurationProperty(FindValueProperty, IsKey = true, DefaultValue = "")]
        public string FindValue
        {
            get { return FindValueElement; }
            set { FindValueElement = value; }
        }

        [ConfigurationProperty(StoreNameProperty, IsKey = true, DefaultValue = StoreName.My)]
        StoreName StoreNameElement
        {
            get { return (StoreName)this[StoreNameProperty]; }
            set { this[StoreNameProperty] = value; }
        }

        [ConfigurationProperty(StoreLocationProperty, IsKey = true, DefaultValue = StoreLocation.LocalMachine)]
        StoreLocation StoreLocationElement
        {
            get { return (StoreLocation)this[StoreLocationProperty]; }
            set { this[StoreLocationProperty] = value; }
        }

        [ConfigurationProperty(X509FindTypeProperty, IsKey = true, DefaultValue = X509FindType.FindBySubjectDistinguishedName)]
        X509FindType X509FindTypeElement
        {
            get { return (X509FindType)this[X509FindTypeProperty]; }
            set { this[X509FindTypeProperty] = value; }
        }

        [ConfigurationProperty(FindValueProperty, IsKey = true, DefaultValue = "")]
        string FindValueElement
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
