using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines a header.
    /// </summary>
    public class ClientHeader : IClientHeader
    {
        /// <summary>
        /// Gets or sets the header's name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the headers's value.
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
