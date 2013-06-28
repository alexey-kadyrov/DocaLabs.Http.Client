using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class NestedTypesPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        readonly PropertyMap _propertyMap;

        NestedTypesPropertyConverter(PropertyInfo property, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, IPropertyConverter> nestedConverters)
            : base(property)
        {
            _propertyMap = new PropertyMap(property.PropertyType, acceptPropertyCheck, nestedConverters);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is not simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, IPropertyConverter> nestedConverters)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            if( type.IsSimpleType() || property.GetIndexParameters().Length > 0)
                return null;

            IPropertyConverter converter;
            return nestedConverters.TryGetValue(type, out converter) 
                ? converter 
                : new NestedTypesPropertyConverter(property, acceptPropertyCheck, nestedConverters);
        }

        /// <summary>
        /// Converts a property value.
        /// If the obj is null or the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null)
                TryAddValues(obj, values);

            return values;
        }

        void TryAddValues(object obj, NameValueCollection values)
        {
            var value = Property.GetValue(obj, null);

            if (value != null)
                values.Add(_propertyMap.Convert(value));
        }
    }
}
