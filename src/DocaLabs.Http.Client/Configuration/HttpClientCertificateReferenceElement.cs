using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines how to find a certificate. 
    /// </summary>
    public class HttpClientCertificateReferenceElement : ConfigurationElement
    {
        const string StoreNameProperty = "storeName";
        const string StoreLocationProperty = "storeLocation";
        const string X509FindTypeProperty = "x509FindType";
        const string FindValueProperty = "findValue";

        /// <summary>
        /// Always returns false letting the element to be modified at runtime.
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// Gets or sets the name of the X.509 certificate store to open.
        /// </summary>
        [ConfigurationProperty(StoreNameProperty, IsKey = true, DefaultValue = StoreName.My)]
        public StoreName StoreName
        {
            get { return (StoreName)this[StoreNameProperty]; }
            set { this[StoreNameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the location of the certificate store the client can use to validate the server’s certificate.
        /// </summary>
        [ConfigurationProperty(StoreLocationProperty, IsKey = true, DefaultValue = StoreLocation.LocalMachine)]
        public StoreLocation StoreLocation
        {
            get { return (StoreLocation)this[StoreLocationProperty]; }
            set { this[StoreLocationProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the type of X.509 search to be executed.
        /// </summary>
        [ConfigurationProperty(X509FindTypeProperty, IsKey = true, DefaultValue = X509FindType.FindBySubjectDistinguishedName)]
        public X509FindType X509FindType
        {
            get { return (X509FindType)this[X509FindTypeProperty]; }
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

        /// <summary>
        /// Finds a certificate referenced by an instance of the class.
        /// </summary>
        /// <returns>Found certificate or null if the certificate is not found, or throws MoreThanOneMatchFoundException if there are more than one match.</returns>
        public X509CertificateCollection Find()
        {
            var certStore = new X509Store(StoreName, StoreLocation);

            try
            {
                certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                return certStore.Certificates.Find(X509FindType, FindValue, true);
            }
            finally 
            {
                certStore.Close();
            }
        }
    }
}
