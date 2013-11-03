namespace DocaLabs.Http.Client.Configuration
{
    public interface IHttpClientEndpointConfiguration
    {
        IClientEndpoint[] Endpoints { get; }
    }
}