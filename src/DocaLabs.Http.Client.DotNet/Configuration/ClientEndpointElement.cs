using System;
using System.ComponentModel;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the http client endpoint. 
    /// </summary>
    public class ClientEndpointElement : ConfigurationElement, IClientEndpoint
    {
        const string NameProperty = "name";
        const string BaseUrlProperty = "baseUrl";
        const string MethodProperty = "method";
        const string TimeoutProperty = "timeout";
        const string AutoSetAcceptEncodingProperty = "autoSetAcceptEncoding";
        const string AuthenticationLevelProperty = "authenticationLevel";
        const string CredentialProperty = "credential";
        const string HeadersProperty = "headers";
        const string ClientCertificatesProperty = "clientCertificates";
        const string ProxyProperty = "proxy";

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        public string Name
        {
            get { return NameElement; }
            set { NameElement = value; }
        }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        public Uri BaseUrl
        {
            get { return BaseUrlElement; }
            set { BaseUrlElement = value; }
        }

        /// <summary>
        /// Gets or sets the request's method, e.g. GET,POST,PUT,DELETE. Default value is empty which means that HttpClient will try to figure out the method.
        /// </summary>
        public string Method
        {
            get { return MethodElement; }
            set { MethodElement = value; }
        }

        /// <summary>
        /// Gets or sets the request timeout in milliseconds. Default value is 90 seconds.
        /// </summary>
        public int Timeout
        {
            get { return TimeoutElement; }
            set { TimeoutElement = value; }
        }

        /// <summary>
        /// Get or sets whenever to add 'Accept-Encoding' header automatically depending on what content decoders are defined in ContentDecoderFactory.
        /// The default value is true.
        /// </summary>
        public bool AutoSetAcceptEncoding
        {
            get { return AutoSetAcceptEncodingElement; }
            set { AutoSetAcceptEncodingElement= value; }
        }

        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        public RequestAuthenticationLevel? AuthenticationLevel
        {
            get { return AuthenticationLevelElement; }
            set { AuthenticationLevelElement = value; }
        }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        public IClientNetworkCredential Credential
        {
            get { return CredentialElement; }
        }

        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        public IClientHeaderCollection Headers
        {
            get { return HeadersElement; }
        }

        /// <summary>
        /// Gets the client certificate collection.
        /// </summary>
        public IClientCertificateReferenceCollection ClientCertificates
        {
            get { return ClientCertificatesElement; }
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        public IClientProxy Proxy
        {
            get { return ProxyElement; }
        }

        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        string NameElement
        {
            get { return ((string)base[NameProperty]); }
            set { base[NameProperty] = value; }
        }

        [ConfigurationProperty(BaseUrlProperty, IsRequired = false), TypeConverter(typeof(UriTypeConverter))]
        Uri BaseUrlElement
        {
            get { return ((Uri)base[BaseUrlProperty]); }
            set { base[BaseUrlProperty] = value; }
        }

        [ConfigurationProperty(MethodProperty, IsRequired = false, DefaultValue = "")]
        string MethodElement
        {
            get { return ((string)base[MethodProperty]); }
            set { base[MethodProperty] = value; }
        }

        [ConfigurationProperty(TimeoutProperty, IsRequired = false, DefaultValue = 90000)]
        int TimeoutElement
        {
            get { return ((int)base[TimeoutProperty]); }
            set { base[TimeoutProperty] = value; }
        }

        [ConfigurationProperty(AutoSetAcceptEncodingProperty, IsRequired = false, DefaultValue = true)]
        bool AutoSetAcceptEncodingElement
        {
            get { return ((bool)base[AutoSetAcceptEncodingProperty]); }
            set { base[AutoSetAcceptEncodingProperty] = value; }
        }

        [ConfigurationProperty(AuthenticationLevelProperty, IsRequired = false, DefaultValue = null)]
        RequestAuthenticationLevel? AuthenticationLevelElement
        {
            get { return ((RequestAuthenticationLevel?)base[AuthenticationLevelProperty]); }
            set { base[AuthenticationLevelProperty] = value; }
        }

        [ConfigurationProperty(CredentialProperty, IsRequired = false)]
        ClientNetworkCredentialElement CredentialElement
        {
            get { return ((ClientNetworkCredentialElement)base[CredentialProperty]); }
        }

        [ConfigurationProperty(HeadersProperty, IsRequired = false)]
        ClientHeaderCollection HeadersElement
        {
            get { return ((ClientHeaderCollection)base[HeadersProperty]); }
        }

        [ConfigurationProperty(ClientCertificatesProperty, IsRequired = false)]
        ClientCertificateReferenceCollection ClientCertificatesElement
        {
            get { return ((ClientCertificateReferenceCollection)base[ClientCertificatesProperty]); }
        }

        [ConfigurationProperty(ProxyProperty, IsRequired = false)]
        ClientProxyElement ProxyElement
        {
            get { return ((ClientProxyElement)base[ProxyProperty]); }
        }
    }
}
