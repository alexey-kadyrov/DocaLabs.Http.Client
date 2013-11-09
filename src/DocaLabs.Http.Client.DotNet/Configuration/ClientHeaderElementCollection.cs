using System;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public class ClientHeaderElementCollection : ConfigurationElementCollectionBase<string, IClientHeader>
    {
        /// <summary>
        /// Initializes a new instance of the ClientHeaderCollection class.
        /// </summary>
        public ClientHeaderElementCollection()
            : base("add")
        {
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IClientHeader) element).Name;
        }

        /// <summary>
        /// Creates a new instance of the element.
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<ClientHeaderElement>();
        }
    }
}
