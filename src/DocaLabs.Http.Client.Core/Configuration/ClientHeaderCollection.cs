using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public class ClientHeaderCollection : IClientHeaderCollection
    {
        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        [XmlElement("header", typeof(ClientHeader))]
        public IClientHeader[] Headers { get; set; }
    }
}
