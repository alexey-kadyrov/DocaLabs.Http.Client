namespace DocaLabs.Http.Client.JsonSerialization
{
    /// <summary>
    /// Defines methods to serialize an object using JSON notation.
    /// /// Note for implementation: all members must be thread safe.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        string Serialize(object obj);
    }
}
