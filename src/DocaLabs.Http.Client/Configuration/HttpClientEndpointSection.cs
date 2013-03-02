using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents the section of a configuration file, which defines a collection of endpoints that a http client can connect to. 
    /// </summary>
    public class HttpClientEndpointSection : ConfigurationSection
    {
        /// <summary>
        /// Gets a list of endpoints that a http client can connect to.
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ClientEndpointCollection Endpoints
        {
            get { return (ClientEndpointCollection)this[""]; }
        }
    }
}
