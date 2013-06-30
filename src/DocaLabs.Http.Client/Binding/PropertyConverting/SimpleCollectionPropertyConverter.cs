using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts enumerable type properties.
    /// </summary>
    public class SimpleCollectionPropertyConverter : PropertyConverterBase, IConverter
    {
        SimpleCollectionPropertyConverter(PropertyInfo property)
            : base(property)
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        /// </summary>
        public static IConverter TryCreate(Type type, PropertyInfo property)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if (property == null)
                throw new ArgumentNullException("property");

            return CanConvert(type)
                ? new SimpleCollectionPropertyConverter(property)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="value">Value of the property.</param>
        /// <returns>One key-values pair.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            if (value != null)
                TryAddValues(value, values);

            return values;
        }

        void TryAddValues(object value, NameValueCollection values)
        {
            var collection = value as IEnumerable;
            if (collection == null)
                return;

            foreach (var v in collection)
                values.Add(Name, ConvertSimpleValue(v));
        }

        static bool CanConvert(Type type)
        {
            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType();
        }
    }
}
