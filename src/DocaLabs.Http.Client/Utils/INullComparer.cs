namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Compares whenever a given value is null. 
    /// </summary>
    /// <remarks>
    /// The interface is defined because ADO.Net is using DBNull.Value as indicator of the null value.
    /// But the DBNull is not supported on all platforms.
    /// </remarks>
    public interface INullComparer
    {
        /// <summary>
        /// Returns true if the value is considered to be null.
        /// </summary>
        bool IsNull(object value);
    }
}
