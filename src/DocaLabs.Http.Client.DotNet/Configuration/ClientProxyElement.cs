using System;
using System.ComponentModel;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the proxy for http client endpoint. 
    /// </summary>
    public class ClientProxyElement : ConfigurationElement, IClientProxy
    {
        const string AddressProperty = "address";
        const string CredentialProperty = "credential";

        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        public Uri Address
        {
            get { return AddressElement; }
            set { AddressElement = value; }
        }

        /// <summary>
        /// Gets or sets the proxy's credentials.
        /// </summary>
        public IClientNetworkCredential Credential
        {
            get { return CredentialElement; }
        }

        [ConfigurationProperty(AddressProperty, IsRequired = false), TypeConverter(typeof(UriTypeConverter))]
        Uri AddressElement
        {
            get { return ((Uri)base[AddressProperty]); }
            set { base[AddressProperty] = value; }
        }

        [ConfigurationProperty(CredentialProperty, IsRequired = false)]
        ClientNetworkCredentialElement CredentialElement
        {
            get { return ((ClientNetworkCredentialElement)base[CredentialProperty]); }
        }
    }
}
