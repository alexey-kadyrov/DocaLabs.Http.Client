using System;
using System.ComponentModel;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the proxy for http client endpoint. 
    /// </summary>
    public class HttpClientProxyElement : ConfigurationElement
    {
        const string AddressProperty = "address";
        const string CredentialProperty = "credential";

        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        [ConfigurationProperty(AddressProperty, IsRequired = false), TypeConverter(typeof(UriTypeConverter))]
        public Uri Address
        {
            get { return ((Uri)base[AddressProperty]); }
            set { base[AddressProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the proxy's credentials.
        /// </summary>
        [ConfigurationProperty(CredentialProperty, IsRequired = false)]
        public NetworkCredentialElement Credential
        {
            get { return ((NetworkCredentialElement)base[CredentialProperty]); }
        }
    }
}
