using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Defines methods to convert a property value for the query mapping.
    /// </summary>
    public interface IConvertProperty
    {
        /// <summary>
        /// Gets a property value.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-value pair with single string which contains all items.</returns>
        CustomNameValueCollection GetValue(object obj);
    }
}
