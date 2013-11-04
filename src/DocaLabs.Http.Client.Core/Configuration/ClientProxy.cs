using System;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the proxy for http client endpoint. 
    /// </summary>
    public class ClientProxy : IClientProxy
    {
        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        [XmlAttribute("address")]
        public Uri Address { get; set; }

        /// <summary>
        /// Gets or sets the proxy's credentials.
        /// </summary>
        [XmlElement("credential", typeof(ClientNetworkCredential))]
        public IClientNetworkCredential Credential { get; set; }
    }
}
