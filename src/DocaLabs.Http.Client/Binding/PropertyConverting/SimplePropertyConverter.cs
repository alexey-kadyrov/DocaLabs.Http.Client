using System;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts simple properties, like int, string, Guid, etc.
    /// </summary>
    public class SimplePropertyConverter : PropertyConverterBase, IConverter 
    {
        SimplePropertyConverter(PropertyInfo property)
            : base(property)
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = Property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is simple
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new SimplePropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the name is white space or obj is null or the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-value pair.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null && (!string.IsNullOrWhiteSpace(Name)))
                TryAddValue(obj, values);

            return values;
        }

        void TryAddValue(object obj, NameValueCollection values)
        {
            var value = Property.GetValue(obj, null);
            if (value != null)
                values.Add(Name, ConvertSimpleValue(value));
        }

        static bool CanConvert(PropertyInfo property)
        {
            return property.PropertyType.IsSimpleType() && property.GetIndexParameters().Length == 0;
        }
    }
}
