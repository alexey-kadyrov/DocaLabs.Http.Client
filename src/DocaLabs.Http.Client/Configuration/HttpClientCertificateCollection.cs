using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of HttpClientCertificateReferenceElement objects.
    /// </summary>
    public class HttpClientCertificateCollection : ConfigurationElementCollectionBase<string, HttpClientCertificateReferenceElement>
    {
        /// <summary>
        /// Initializes a new instance of the HttpClientCertificateCollection class.
        /// </summary>
        public HttpClientCertificateCollection()
            : base("certificateReference")
        {
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var value = (HttpClientCertificateReferenceElement) element;
            return string.Format("{0}-{1}-{2}-{3}", value.StoreName, value.StoreLocation, value.X509FindType, value.FindValue);
        }
    }
}
