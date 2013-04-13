using System;
using System.Collections;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts enumerable type properties.
    /// </summary>
    public class CollectionPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        CollectionPropertyConverter(PropertyInfo property, IPropertyConverterOverrides overrides)
            : base(property, overrides)
        {
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0
                ? new CollectionPropertyConverter(property, overrides)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the name is white space or obj is null or the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-values pair.</returns>
        public CustomNameValueCollection Convert(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null && (!string.IsNullOrWhiteSpace(Name)))
                TryAddValues(obj, values);

            return values;
        }

        void TryAddValues(object obj, IDictionaryList<string, string> values)
        {
            var collection = Property.GetValue(obj, null) as IEnumerable;
            if (collection == null)
                return;

            foreach (var value in collection)
                values.Add(Name, ConvertSimpleValue(value));
        }
    }
}
