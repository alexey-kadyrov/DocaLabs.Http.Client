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
                Name = Property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new SimpleCollectionPropertyConverter(property)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-values pair.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null)
                TryAddValues(obj, values);

            return values;
        }

        void TryAddValues(object obj, NameValueCollection values)
        {
            var collection = Property.GetValue(obj, null) as IEnumerable;
            if (collection == null)
                return;

            foreach (var value in collection)
                values.Add(Name, ConvertSimpleValue(value));
        }

        static bool CanConvert(PropertyInfo property)
        {
            var type = property.PropertyType;

            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0;
        }
    }
}
