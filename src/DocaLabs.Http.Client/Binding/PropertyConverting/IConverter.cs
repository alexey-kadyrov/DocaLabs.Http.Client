using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to convert a property value to name-value pairs.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts a property value.
        /// </summary>
        /// <param name="value">Value of the property.</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        NameValueCollection Convert(object value);
    }
}
