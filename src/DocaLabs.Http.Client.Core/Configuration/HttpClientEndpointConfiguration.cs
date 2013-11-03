using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents the section of a configuration file, which defines a collection of endpoints that a http client can connect to. 
    /// </summary>
    [XmlRoot(
        ElementName = "httpClientEndpointConfiguration", 
        IsNullable = true,
        Namespace = "http://docalabshttpclient.codeplex.com/v1/")]
    public class HttpClientEndpointConfiguration : IHttpClientEndpointConfiguration
    {
        /// <summary>
        /// Gets a list of endpoints that a http client can connect to.
        /// </summary>
        [XmlElement("endpoint", typeof(ClientEndpoint[]))]
        public IClientEndpoint[] Endpoints { get; set; }
    }
}
