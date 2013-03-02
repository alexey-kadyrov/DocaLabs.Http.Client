using System;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public class ClientHeaderCollection : ConfigurationElementCollectionBase<string, IClientHeader>, IClientHeaderCollection
    {
        /// <summary>
        /// Initializes a new instance of the ClientHeaderCollection class.
        /// </summary>
        public ClientHeaderCollection()
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
