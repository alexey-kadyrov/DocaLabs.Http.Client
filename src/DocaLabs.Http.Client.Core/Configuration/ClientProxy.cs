using System;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the proxy for http client endpoint. 
    /// </summary>
    public class ClientProxy : IClientProxy
    {
        ClientNetworkCredential _credentialXmlData;

        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        [XmlIgnore]
        public Uri Address { get; private set; }

        [XmlAttribute("address")]
        public string AddressXmlData
        {
            get
            {
                var address = Address;
                return address != null ? address.OriginalString : null;
            }
            set { Address = string.IsNullOrWhiteSpace(value) ? null : new Uri(value); }
        }

        /// <summary>
        /// Gets or sets the proxy's credentials.
        /// </summary>
        [XmlIgnore]
        public IClientNetworkCredential Credential { get { return CredentialXmlData; } }

        [XmlElement("credential")]
        public ClientNetworkCredential CredentialXmlData
        {
            get { return _credentialXmlData; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _credentialXmlData = value;
            }
        }

        public ClientProxy()
        {
            _credentialXmlData = new ClientNetworkCredential();
        }
    }
}
