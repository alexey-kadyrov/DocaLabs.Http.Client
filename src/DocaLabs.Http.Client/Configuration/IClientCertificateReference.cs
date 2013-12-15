namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines how to find a certificate. 
    /// </summary>
    public interface IClientCertificateReference
    {
        /// <summary>
        /// Gets or sets the name of the X.509 certificate store to open.
        /// </summary>
        CertificateStoreName StoreName { get; set; }

        /// <summary>
        /// Gets or sets a value that specifies the location of the certificate store the client can use to validate the server’s certificate.
        /// </summary>
        CertificateStoreLocation StoreLocation { get; set; }

        /// <summary>
        /// Gets or sets the type of X.509 search to be executed.
        /// </summary>
        CertificateX509FindType X509FindType { get; set; }

        /// <summary>
        /// Gets or sets a string that specifies the value to search for in the X.509 certificate store.
        /// </summary>
        string FindValue { get; set; }
    }
}
