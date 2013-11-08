using System.Collections.Generic;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Configuration
{
    public class ClientCertificateReferenceCollection
    {
        [XmlIgnore]
        public IReadOnlyList<IClientCertificateReference> References { get; private set; }

        [XmlElement("certificateReference")]
        public List<ClientCertificateReference> ReferencesXmlData { get; private set; }

        public ClientCertificateReferenceCollection()
        {
            ReferencesXmlData = new List<ClientCertificateReference>();
            References = new ReadOnlyList<IClientCertificateReference, ClientCertificateReference>(ReferencesXmlData);
        }
    }
}
