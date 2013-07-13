using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to convert a value to name-value pairs. It's mainly used by property converters.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        NameValueCollection Convert(object value);
    }
}
