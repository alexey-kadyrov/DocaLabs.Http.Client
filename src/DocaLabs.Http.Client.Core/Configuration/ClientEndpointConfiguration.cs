using System.Collections.Generic;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents the section of a configuration file, which defines a collection of endpoints that a http client can connect to. 
    /// </summary>
    [XmlRoot(ElementName = "httpClientEndpointConfiguration")]
    public class ClientEndpointConfiguration : IClientEndpointConfiguration
    {
        [XmlIgnore]
        public IReadOnlyList<IClientEndpoint> Endpoints { get; private set; }

        [XmlElement("endpoint")]
        public List<ClientEndpoint> EndpointsXmlData { get; private set; }

        public ClientEndpointConfiguration()
        {
            EndpointsXmlData = new List<ClientEndpoint>();
            Endpoints = new ReadOnlyList<IClientEndpoint, ClientEndpoint>(EndpointsXmlData);
        }
    }
}
