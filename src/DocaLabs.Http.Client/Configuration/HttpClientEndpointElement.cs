using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the http client endpoint. 
    /// </summary>
    public class HttpClientEndpointElement : ConfigurationElement
    {
        const string NameProperty = "name";
        const string BaseUrlProperty = "baseUrl";
        const string TimeoutProperty = "timeout";
        const string AutoSetAcceptEncodingProperty = "autoSetAcceptEncoding";
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
        /// Gets or sets the request timeout. Default value is 90 seconds.
        /// </summary>
        [ConfigurationProperty(TimeoutProperty, IsRequired = false, DefaultValue = 90000)]
        public int Timeout
        {
            get { return ((int)base[TimeoutProperty]); }
            set { base[TimeoutProperty] = value; }
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
        /// Gets the headers collection.
        /// </summary>
        [ConfigurationProperty(HeadersProperty, IsRequired = false)]
        public NameValueConfigurationCollection Headers
        {
            get { return ((NameValueConfigurationCollection)base[HeadersProperty]); }
        }

        /// <summary>
        /// Gets the client certificate collection.
        /// </summary>
        [ConfigurationProperty(ClientCertificatesProperty, IsRequired = false)]
        public HttpClientCertificateCollection ClientCertificates
        {
            get { return ((HttpClientCertificateCollection)base[ClientCertificatesProperty]); }
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        [ConfigurationProperty(ProxyProperty, IsRequired = false)]
        public HttpClientProxyElement Proxy
        {
            get { return ((HttpClientProxyElement)base[ProxyProperty]); }
        }

        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        public void CopyHeadersTo(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (var name in Headers.AllKeys)
                request.Headers.Add(name, Headers[name].Value);
        }

        /// <summary>
        /// If client certificates are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        public void CopyClientCertificatesTo(HttpWebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            foreach (HttpClientCertificateReferenceElement certRef in ClientCertificates)
                request.ClientCertificates.AddRange(certRef.Find());
        }

        /// <summary>
        /// If web proxy are defined in the endpoint configuration then the methods adds it to the request.
        /// </summary>
        public void CopyWebProxyTo(WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (Proxy != null && Proxy.Address != null)
                request.Proxy = new WebProxy(Proxy.Address);
        }
    }
}
