using System;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the http client endpoint. 
    /// </summary>
    public class ClientEndpoint : IClientEndpoint
    {
        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        [XmlAttribute("baseUrl")]
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the request's method, e.g. GET,POST,PUT,DELETE. Default value is empty which means that HttpClient will try to figure out the method.
        /// </summary>
        [XmlAttribute("method")]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the request timeout in milliseconds. Default value is 90 seconds.
        /// </summary>
        [XmlAttribute("timeout")]
        public int Timeout { get; set; }

        /// <summary>
        /// Get or sets whenever to add 'Accept-Encoding' header automatically depending on what content decoders are defined in ContentDecoderFactory.
        /// The default value is true.
        /// </summary>
        [XmlAttribute("autoSetAcceptEncoding")]
        public bool AutoSetAcceptEncoding { get; set; }

        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        [XmlAttribute("authenticationLevel")]
        public RequestAuthenticationLevel? AuthenticationLevel { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        [XmlElement("credential", typeof(ClientNetworkCredential))]
        public IClientNetworkCredential Credential { get; set; }

        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        [XmlElement("headers")]
        public IClientHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets the client certificate collection.
        /// </summary>
        [XmlElement("clientCertificates")]
        public IClientCertificateReferenceCollection ClientCertificates { get; set; }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        [XmlElement("proxy")]
        public IClientProxy Proxy { get; set; }
    }
}
