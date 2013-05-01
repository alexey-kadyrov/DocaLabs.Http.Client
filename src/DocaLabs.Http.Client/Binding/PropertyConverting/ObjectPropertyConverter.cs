using System;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class ObjectPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        readonly Func<object, PropertyMap> _propertyMapGetOrAddType;

        ObjectPropertyConverter(PropertyInfo property, IPropertyConverterOverrides overrides, Func<object, PropertyMap> propertyMapGetOrAddType)
            : base(property, overrides)
        {
            _propertyMapGetOrAddType = propertyMapGetOrAddType;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is not simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, IPropertyConverterOverrides overrides, Func<object, PropertyMap> propertyMapGetOrAddType)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return property.PropertyType.IsSimpleType() || property.GetIndexParameters().Length > 0
                ? null
                : new ObjectPropertyConverter(property, overrides, propertyMapGetOrAddType);
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
                values.Add(_propertyMapGetOrAddType(value).ConvertModel(value));
        }
    }
}
