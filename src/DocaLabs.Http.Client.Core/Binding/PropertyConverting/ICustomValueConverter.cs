using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to convert a value to name-value pairs.
    /// It can be implemented by an object.
    /// </summary>
    public interface ICustomValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        NameValueCollection ConvertProperties();
    }
}
