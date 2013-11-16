using System;
using System.Collections.Generic;
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
        const string RequestTimeoutProperty = "timeout";
        const string ReadTimeoutProperty = "readTimeout";
        const string WriteTimeoutProperty = "writeTimeout";
        const string AutoSetAcceptEncodingProperty = "autoSetAcceptEncoding";
        const string AuthenticationLevelProperty = "authenticationLevel";
        const string CredentialProperty = "credential";
        const string HeadersProperty = "headers";
        const string ClientCertificatesProperty = "clientCertificates";
        const string ProxyProperty = "proxy";

        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)base[NameProperty]); }
            set { base[NameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        [ConfigurationProperty(BaseUrlProperty, IsRequired = false), TypeConverter(typeof(UriTypeConverter))]
        public Uri BaseUrl
        {
            get { return ((Uri)base[BaseUrlProperty]); }
            set { base[BaseUrlProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the request's method, e.g. GET,POST,PUT,DELETE. Default value is empty which means that HttpClient will try to figure out the method.
        /// </summary>
        [ConfigurationProperty(MethodProperty, IsRequired = false, DefaultValue = "")]
        public string Method
        {
            get { return ((string)base[MethodProperty]); }
            set { base[MethodProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the request timeout in milliseconds. Default value is 90 seconds.
        /// </summary>
        [ConfigurationProperty(RequestTimeoutProperty, IsRequired = false, DefaultValue = 90000)]
        public int Timeout
        {
            get { return ((int)base[RequestTimeoutProperty]); }
            set { base[RequestTimeoutProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the response stream reading timeout in milliseconds. Default value is 300 seconds.
        /// </summary>
        [ConfigurationProperty(ReadTimeoutProperty, IsRequired = false, DefaultValue = 300000)]
        public int ReadTimeout
        {
            get { return ((int)base[ReadTimeoutProperty]); }
            set { base[ReadTimeoutProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the request stream writing timeout in milliseconds. Default value is 300 seconds.
        /// </summary>
        [ConfigurationProperty(WriteTimeoutProperty, IsRequired = false, DefaultValue = 300000)]
        public int WriteTimeout
        {
            get { return ((int)base[WriteTimeoutProperty]); }
            set { base[WriteTimeoutProperty] = value; }
        }

        /// <summary>
        /// Get or sets whenever to add 'Accept-Encoding' header automatically depending on what content decoders are defined in ContentDecoderFactory.
        /// The default value is true.
        /// </summary>
        [ConfigurationProperty(AutoSetAcceptEncodingProperty, IsRequired = false, DefaultValue = true)]
        public bool AutoSetAcceptEncoding
        {
            get { return ((bool)base[AutoSetAcceptEncodingProperty]); }
            set { base[AutoSetAcceptEncodingProperty] = value; }
        }

        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        [ConfigurationProperty(AuthenticationLevelProperty, IsRequired = false, DefaultValue = RequestAuthenticationLevel.Undefined)]
        public RequestAuthenticationLevel AuthenticationLevel
        {
            get { return ((RequestAuthenticationLevel)base[AuthenticationLevelProperty]); }
            set { base[AuthenticationLevelProperty] = value; }
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
        public IReadOnlyList<IClientHeader> Headers
        {
            get { return HeadersElement; }
        }

        /// <summary>
        /// Gets the client certificate collection.
        /// </summary>
        public IReadOnlyList<IClientCertificateReference> ClientCertificates
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

        [ConfigurationProperty(CredentialProperty, IsRequired = false)]
        ClientNetworkCredentialElement CredentialElement
        {
            get { return ((ClientNetworkCredentialElement)base[CredentialProperty]); }
        }

        [ConfigurationProperty(HeadersProperty, IsRequired = false)]
        ClientHeaderElementCollection HeadersElement
        {
            get { return ((ClientHeaderElementCollection)base[HeadersProperty]); }
        }

        [ConfigurationProperty(ClientCertificatesProperty, IsRequired = false)]
        ClientCertificateReferenceElementCollection ClientCertificatesElement
        {
            get { return ((ClientCertificateReferenceElementCollection)base[ClientCertificatesProperty]); }
        }

        [ConfigurationProperty(ProxyProperty, IsRequired = false)]
        ClientProxyElement ProxyElement
        {
            get { return ((ClientProxyElement)base[ProxyProperty]); }
        }
    }
}
