using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.Mapping
{
    /// <summary>
    /// Defines methods to convert query class to URL's query string.
    /// The interface can be implemented by the query class in order to supply custom conversion.
    /// </summary>
    public interface ICustomQueryMapper
    {
        /// <summary>
        /// Serializes the instance into the collection of pairs: parameter name and its values.
        /// </summary>
        /// <returns></returns>
        CustomNameValueCollection ToParameterDictionary();
    }
}
