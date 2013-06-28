using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converter for enumerable properties that serializes into delimited string.
    /// </summary>
    public class SeparatedCollectionConverter : PropertyConverterBase, IPropertyConverter
    {
        /// <summary>
        /// String's delimiter. The default value is pipe |.
        /// </summary>
        public char Separator { get; set; }

        SeparatedCollectionConverter(PropertyInfo property, char separator)
            : base(property)
        {
            if (property.GetIndexParameters().Length > 0)
                throw new ArgumentException(Resources.Text.property_cannot_be_indexer, "property");

            Separator = separator;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, char separator = '|')
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0
                ? new SeparatedCollectionConverter(property, separator)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the name is white space or obj is null or the value of the property is null (or eventually empty string) then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-value pair with single string as value which contains all items.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null && (!string.IsNullOrWhiteSpace(Name)))
            {
                TryAddValues(obj, values);
            }

            return values;
        }

        void TryAddValues(object obj, NameValueCollection values)
        {
            var collection = Property.GetValue(obj, null) as IEnumerable;

            if (collection != null)
                TryBuildString(values, collection);
        }

        void TryBuildString(NameValueCollection values, IEnumerable collection)
        {
            var stringBuilder = new StringBuilder();

            var first = true;

            foreach (var value in collection)
            {
                if (!first)
                    stringBuilder.Append(Separator);

                stringBuilder.Append(ConvertSimpleValue(value));

                first = false;
            }

            if (stringBuilder.Length > 0)
                values.Add(Name, stringBuilder.ToString());
        }
    }
}
