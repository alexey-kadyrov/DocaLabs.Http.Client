using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    public class ClientCertificateReference : IClientCertificateReference
    {
        [XmlAttribute("storeName")]
        public CertificateStoreName StoreName { get; set; }

        [XmlAttribute("storeLocation")]
        public CertificateStoreLocation StoreLocation { get; set; }

        [XmlAttribute("x509FindType")]
        public CertificateX509FindType X509FindType { get; set; }

        [XmlAttribute("findValue")]
        public string FindValue { get; set; }

        public ClientCertificateReference()
        {
            StoreName = CertificateStoreName.My;
            StoreLocation = CertificateStoreLocation.LocalMachine;
            X509FindType = CertificateX509FindType.FindBySubjectDistinguishedName;
            FindValue = string.Empty;
        }
    }
}
