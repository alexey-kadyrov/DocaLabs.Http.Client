namespace DocaLabs.Http.Client.Configuration
{
    public enum RequestAuthenticationLevel
    {
        Undefined = 0,
        None = 1,
        MutualAuthRequested = 2,
        MutualAuthRequired = 3,
    }
}
