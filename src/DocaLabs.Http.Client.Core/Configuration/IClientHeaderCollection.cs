namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public interface IClientHeaderCollection
    {
        IClientHeader[] Headers { get; set; }
    }
}