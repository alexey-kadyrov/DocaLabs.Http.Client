using System;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the proxy for http client endpoint. 
    /// </summary>
    public interface IClientProxy
    {
        /// <summary>
        /// Gets or sets the proxy address.
        /// </summary>
        Uri Address { get; }

        /// <summary>
        /// Gets or sets the proxy's credentials.
        /// </summary>
        IClientNetworkCredential Credential { get; }
    }
}