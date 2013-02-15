using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of HttpClientEndpointElement objects.
    /// </summary>
    public class HttpClientEndpointCollection : ConfigurationElementCollectionBase<string, HttpClientEndpointElement>
    {
        /// <summary>
        /// Initializes a new instance of the HttpClientEndpointCollection class.
        /// </summary>
        public HttpClientEndpointCollection()
            : base("endpoint")
        {
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HttpClientEndpointElement)element).Name;
        }
    }
}
