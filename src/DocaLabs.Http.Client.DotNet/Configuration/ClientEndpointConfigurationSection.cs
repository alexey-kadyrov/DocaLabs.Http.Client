using System.Collections.Generic;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents the section of a configuration file, which defines a collection of endpoints that a http client can connect to. 
    /// </summary>
    public class ClientEndpointConfigurationSection : ConfigurationSection, IClientEndpointConfiguration
    {
        /// <summary>
        /// Gets a list of endpoints that a http client can connect to.
        /// </summary>
        public IReadOnlyList<IClientEndpoint> Endpoints
        {
            get { return EndpointsElement; }
        }

        /// <summary>
        /// Gets a list of endpoints that a http client can connect to.
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        ClientEndpointElementCollection EndpointsElement
        {
            get { return (ClientEndpointElementCollection)this[""]; }
        }
    }
}
