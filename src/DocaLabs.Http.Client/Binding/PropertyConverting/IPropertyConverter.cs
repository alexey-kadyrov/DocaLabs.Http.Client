using System.Collections.Generic;
using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to convert a property value to name-value pairs.
    /// </summary>
    public interface IPropertyConverter
    {
        /// <summary>
        /// Converts a property value.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">List of object (values which are not int, string, etc.) that were processed in order to prevent circular references.</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        NameValueCollection Convert(object instance, ISet<object> processed);
    }
}
