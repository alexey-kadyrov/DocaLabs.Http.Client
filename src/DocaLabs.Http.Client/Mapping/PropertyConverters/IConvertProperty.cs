using System.Collections.Generic;

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
        IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj);
    }
}
