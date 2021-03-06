﻿using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines methods to get an endpoint configuration using the application/web configuration file.
    /// </summary>
    public class EndpointConfigurationProvider : IEndpointConfigurationProvider
    {
        /// <summary>
        /// Default name for the configuration section.
        /// </summary>
        public const string DefaultFileName = "HttpClientEndpoints.xml";

        string _fileName = DefaultFileName;
        IClientEndpointConfiguration _configuration;
        readonly object _locker = new object();

        /// <summary>
        /// Defines a section name the root element with endpoint configurations.
        /// The default name is 'httpClientEndpoints'.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _fileName = value;
            }
        }

        public IClientEndpointConfiguration Configuration
        {
            get
            {
                lock (_locker)
                {
                    return _configuration ?? (_configuration = Load(_fileName));
                }
            }
        }

        /// <summary>
        /// Returns an endpoint configuration for provided name.
        /// </summary>
        public IClientEndpoint GetEndpoint(string configurationName)
        {
            return Configuration.Endpoints.FirstOrDefault(x => x.Name == configurationName);
        }

        static IClientEndpointConfiguration Load(string file)
        {
            try
            {
                using (var reader = XmlReader.Create(file))
                {
                    return (IClientEndpointConfiguration)(new XmlSerializer(typeof(ClientEndpointConfiguration)).Deserialize(reader));
                }
            }
            catch (FileNotFoundException)
            {
                return new ClientEndpointConfiguration();
            }
        }
    }
}