namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines a header.
    /// </summary>
    public interface IClientHeader
    {
        /// <summary>
        /// Gets or sets the header's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the headers's value.
        /// </summary>
        string Value { get; }
    }
}