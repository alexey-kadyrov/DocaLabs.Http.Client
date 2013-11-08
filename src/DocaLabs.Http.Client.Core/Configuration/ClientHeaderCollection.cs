using System.Collections.Generic;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public class ClientHeaderCollection
    {
        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        [XmlIgnore]
        public IReadOnlyList<IClientHeader> Headers { get; private set; }

        [XmlElement("add")]
        public List<ClientHeader> HeadersXmlData { get; private set; }

        public ClientHeaderCollection()
        {
            HeadersXmlData = new List<ClientHeader>();
            Headers = new ReadOnlyList<IClientHeader, ClientHeader>(HeadersXmlData);
        }
    }
}
