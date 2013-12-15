using System;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Holds current endpoint configuration provider.
    /// </summary>
    public static class EndpointConfigurationFactory
    {
        static IEndpointConfigurationProvider _current = PlatformAdapter.Resolve<IEndpointConfigurationProvider>();

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
