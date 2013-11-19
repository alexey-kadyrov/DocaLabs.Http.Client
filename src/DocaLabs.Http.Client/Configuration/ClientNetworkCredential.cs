using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the network credentials. 
    /// </summary>
    public class ClientNetworkCredential : IClientNetworkCredential
    {
        /// <summary>
        /// Gets or sets the type of credentials. If it's DefaultCredentials or DefaultNetworkCredentials other properties will be ignored.
        /// The default value is CredentialType.None.
        /// </summary>
        [XmlAttribute("credentialType")]
        public CredentialType CredentialType { get; set; }

        /// <summary>
        /// Gets or sets the user name if the CredentialType is NetworkCredential.
        /// </summary>
        [XmlAttribute("user")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the password if the CredentialType is NetworkCredential.
        /// </summary>
        [XmlAttribute("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the domain if the CredentialType is NetworkCredential.
        /// </summary>
        [XmlAttribute("domain")]
        public string Domain { get; set; }
    }
}
