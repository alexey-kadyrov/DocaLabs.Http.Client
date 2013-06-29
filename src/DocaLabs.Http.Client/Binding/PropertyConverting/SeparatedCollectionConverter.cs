using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converter for enumerable properties that serializes their values into delimited string.
    /// </summary>
    public class SeparatedCollectionConverter : PropertyConverterBase, IConverter
    {
        /// <summary>
        /// String's delimiter.
        /// </summary>
        public char Separator { get; set; }

        SeparatedCollectionConverter(PropertyInfo property, char separator)
            : base(property)
        {
            Separator = separator;

            if (string.IsNullOrWhiteSpace(Name))
                Name = Property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(PropertyInfo property, char separator = '|')
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            return CanConvert(property, type)
                ? new SeparatedCollectionConverter(property, separator)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the value of the property is null (or eventually empty string) then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-value pair with single string as value which contains all items separated by the provided separator.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null)
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

        static bool CanConvert(PropertyInfo property, Type type)
        {
            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0;
        }
    }
}
