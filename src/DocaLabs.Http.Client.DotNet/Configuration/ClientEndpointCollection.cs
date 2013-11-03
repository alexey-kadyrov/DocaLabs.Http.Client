using System;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of ClientEndpointElement objects.
    /// </summary>
    public class ClientEndpointCollection : ConfigurationElementCollectionBase<string, ClientEndpointElement>
    {
        /// <summary>
        /// Initializes a new instance of the ClientEndpointCollection class.
        /// </summary>
        public ClientEndpointCollection()
            : base("endpoint")
        {
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClientEndpointElement)element).Name;
        }

        /// <summary>
        /// Creates a new instance of the element.
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<ClientEndpointElement>();
        }
    }
}
