using System;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of ClientCertificateReferenceElement objects.
    /// </summary>
    public class ClientCertificateReferenceCollection : ConfigurationElementCollectionBase<string, IClientCertificateReference>, IClientCertificateReferenceCollection
    {
        /// <summary>
        /// Initializes a new instance of the ClientCertificateReferenceCollection class.
        /// </summary>
        public ClientCertificateReferenceCollection()
            : base("certificateReference")
        {
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var value = (IClientCertificateReference)element;
            return string.Format("{0}-{1}-{2}-{3}", value.StoreName, value.StoreLocation, value.X509FindType, value.FindValue);
        }

        /// <summary>
        /// Creates a new instance of the element.
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<ClientCertificateReferenceElement>();
        }
    }
}
