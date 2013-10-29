using System;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Holds current endpoint configuration provider.
    /// </summary>
    public static class EndpointConfiguration
    {
        static IEndpointConfigurationProvider _current = new DefaultEndpointConfigurationProvider();

        /// <summary>
        /// Current endpoint configuration provider.
        /// </summary>
        public static IEndpointConfigurationProvider Current
        {
            get { return _current; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _current = value;
            }
        }
    }
}
