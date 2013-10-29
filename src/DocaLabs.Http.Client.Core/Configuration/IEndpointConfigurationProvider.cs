namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines methods to get an endpoint configuration
    /// </summary>
    public interface IEndpointConfigurationProvider
    {
        /// <summary>
        /// Returns an endpoint configuration for provided name.
        /// </summary>
        IClientEndpoint GetEndpoint(string configurationName);
    }
}
