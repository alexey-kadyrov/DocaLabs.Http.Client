using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the http client endpoint. 
    /// </summary>
    public class ClientEndpoint : IClientEndpoint
    {
        ClientNetworkCredential _credentialXmlData;
        ClientHeaderCollection _headersXmlData;
        ClientCertificateReferenceCollection _clientCertificatesXmlData;
        ClientProxy _proxyXmlData;

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        [XmlIgnore]
        public Uri BaseUrl { get; private set; }

        [XmlAttribute("baseUrl")]
        public string BaseUrlXmlData
        {
            get
            {
                var baseUrl = BaseUrl;
                return baseUrl != null ? baseUrl.OriginalString : null;
            }
            set { BaseUrl = string.IsNullOrWhiteSpace(value) ? null : new Uri(value); }
        }

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
        public RequestAuthenticationLevel AuthenticationLevel { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        [XmlIgnore]
        public IClientNetworkCredential Credential { get { return CredentialXmlData; } }

        [XmlElement("credential")]
        public ClientNetworkCredential CredentialXmlData
        {
            get { return _credentialXmlData; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _credentialXmlData = value;
            }
        }

        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        [XmlIgnore]
        public IReadOnlyList<IClientHeader> Headers { get { return HeadersXmlData.Headers; } }

        [XmlElement("headers")]
        public ClientHeaderCollection HeadersXmlData
        {
            get { return _headersXmlData; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _headersXmlData = value;
            }
        }

        [XmlIgnore]
        public IReadOnlyList<IClientCertificateReference> ClientCertificates { get { return ClientCertificatesXmlData.References; } }

        [XmlElement("clientCertificates")]
        public ClientCertificateReferenceCollection ClientCertificatesXmlData
        {
            get { return _clientCertificatesXmlData; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _clientCertificatesXmlData = value;
            }
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        [XmlIgnore]
        public IClientProxy Proxy { get { return ProxyXmlData; } }

        [XmlElement("proxy")]
        public ClientProxy ProxyXmlData
        {
            get { return _proxyXmlData; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _proxyXmlData = value;
            }
        }

        public ClientEndpoint()
        {
            _headersXmlData = new ClientHeaderCollection();
            _credentialXmlData = new ClientNetworkCredential();
            _proxyXmlData = new ClientProxy();
            _clientCertificatesXmlData = new ClientCertificateReferenceCollection();

            Method = string.Empty;
            Timeout = 90000;
            AutoSetAcceptEncoding = true;
            AuthenticationLevel = RequestAuthenticationLevel.Undefined;
        }
    }
}
