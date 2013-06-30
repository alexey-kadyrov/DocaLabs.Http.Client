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
                Name = property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is simple
        /// </summary>
        public static IConverter TryCreate(Type type, PropertyInfo property)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if (property == null)
                throw new ArgumentNullException("property");

            return CanConvert(type)
                ? new SimplePropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// </summary>
        /// <param name="value">Value of the property.</param>
        /// <returns>One key-value pair. If the value of the property is null then the return collection will be empty.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            if (value != null)
                values.Add(Name, ConvertSimpleValue(value));

            return values;
        }

        static bool CanConvert(Type type)
        {
            return type.IsSimpleType();
        }
    }
}
