using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines a header.
    /// </summary>
    public class ClientHeaderElement : ConfigurationElement, IClientHeader
    {
        const string NameProperty = "name";
        const string ValueProperty = "value";

        /// <summary>
        /// Gets or sets the header's name.
        /// </summary>
        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)base[NameProperty]); }
            set { base[NameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the headers's value.
        /// </summary>
        [ConfigurationProperty(ValueProperty, IsRequired = true)]
        public string Value
        {
            get { return ((string)base[ValueProperty]); }
            set { base[ValueProperty] = value; }
        }
    }
}
