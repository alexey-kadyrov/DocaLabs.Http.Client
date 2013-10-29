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
        public string Name
        {
            get { return NameElement; }
            set { NameElement = value; }
        }

        /// <summary>
        /// Gets or sets the headers's value.
        /// </summary>
        public string Value
        {
            get { return ValueElement; }
            set { ValueElement = value; }
        }

        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        string NameElement
        {
            get { return ((string)base[NameProperty]); }
            set { base[NameProperty] = value; }
        }

        [ConfigurationProperty(ValueProperty, IsRequired = true)]
        string ValueElement
        {
            get { return ((string)base[ValueProperty]); }
            set { base[ValueProperty] = value; }
        }
    }
}
