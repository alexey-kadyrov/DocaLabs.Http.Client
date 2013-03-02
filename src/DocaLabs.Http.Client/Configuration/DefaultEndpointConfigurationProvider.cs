using System;
using System.Configuration;
using System.IO;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines methods to get an endpoint configuration
    /// </summary>
    public class DefaultEndpointConfigurationProvider : IEndpointConfigurationProvider
    {
        /// <summary>
        /// Default name for the configuration section.
        /// </summary>
        public const string DefaultSectionName = "httpClientEndpoints";

        System.Configuration.Configuration _configuration;
        string _sectionName = DefaultSectionName;

        /// <summary>
        /// Defines a section name the root element with endpoint configurations.
        /// The default name is 'httpClientEndpoints'.
        /// </summary>
        public string SectionName
        {
            get { return _sectionName; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _sectionName = value;
            }
        }

        /// <summary>
        /// Sets a specified file as a source for the configuration.
        /// </summary>
        public void SetSource(string fileName)
        {
            if (fileName == null)
            {
                _configuration = null;
                return;
            }

            if (!File.Exists(fileName))
                throw new FileNotFoundException(string.Format(Resources.Text.configuration_file_0_not_found, fileName), fileName);

            _configuration = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = fileName }, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Returns an endpoint configuration for provided name.
        /// </summary>
        public IClientEndpoint GetEndpoint(string configurationName)
        {
            var section = GetSection();

            return section == null
               ? null
               : section.Endpoints[configurationName];
        }

        HttpClientEndpointSection GetSection()
        {
            return _configuration == null
                ? ConfigurationManager.GetSection(_sectionName) as HttpClientEndpointSection
                : _configuration.GetSection(_sectionName) as HttpClientEndpointSection;
        }
    }
}