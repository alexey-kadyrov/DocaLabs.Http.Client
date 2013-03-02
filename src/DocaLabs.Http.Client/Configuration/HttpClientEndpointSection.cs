using System;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents the section of a configuration file, which defines a collection of endpoints that a http client can connect to. 
    /// </summary>
    public class HttpClientEndpointSection : ConfigurationSection
    {
        /// <summary>
        /// The default section name.
        /// </summary>
        public const string DefaultSectionName = "httpClientEndpoints";

        /// <summary>
        /// Gets a list of endpoints that a http client can connect to.
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ClientEndpointCollection Endpoints
        {
            get { return (ClientEndpointCollection)this[""]; }
        }

        /// <summary>
        /// Retrieves a HttpClientEndpointSection configuration section using the default name.
        /// </summary>
        /// <returns>The specified ConfigurationSection object, or null if the section does not exist.</returns>
        public static HttpClientEndpointSection GetDefaultSection()
        {
            return GetSection(DefaultSectionName);
        }

        /// <summary>
        ///  Retrieves a specified HttpClientEndpointSection configuration section.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <returns>The specified ConfigurationSection object, or null if the section does not exist.</returns>
        public static HttpClientEndpointSection GetSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentNullException("sectionName");

            return ConfigurationManager.GetSection(sectionName) as HttpClientEndpointSection;
        }
    }
}
