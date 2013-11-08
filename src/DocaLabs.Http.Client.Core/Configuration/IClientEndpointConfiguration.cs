using System.Collections.Generic;

namespace DocaLabs.Http.Client.Configuration
{
    public interface IClientEndpointConfiguration
    {
        IReadOnlyList<IClientEndpoint> Endpoints { get; }
    }
}