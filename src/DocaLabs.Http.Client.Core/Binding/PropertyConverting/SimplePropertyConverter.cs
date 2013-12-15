using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts simple properties, like int, string, Guid, etc.
    /// </summary>
    public class SimplePropertyConverter :  IPropertyConverter
    {
        readonly PropertyInfo _property;
        readonly IValueConverter _valueConverter;

        SimplePropertyConverter(PropertyInfo property)
        {
            _property = property;

            string name = null, format = null;
            CultureInfo culture = null;

            var requestUse = property.GetCustomAttribute<PropertyOverridesAttribute>();
            if (requestUse != null)
            {
                name = requestUse.Name;
                format = requestUse.Format;
                culture = requestUse.GetFormatCultureInfo();
            }

            if (string.IsNullOrWhiteSpace(name))
                name = _property.Name;

            _valueConverter = new SimpleValueConverter(name, format, culture);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new SimplePropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">Ignored.</param>
        /// <returns>One key-value pair.</returns>
        public ICustomKeyValueCollection Convert(object instance, ISet<object> processed)
        {
            return instance == null
                ? new CustomKeyValueCollection() 
                : _valueConverter.Convert(_property.GetValue(instance));
        }

        static bool CanConvert(PropertyInfo property)
        {
            return !property.IsIndexer() && property.GetMethod != null && SimpleValueConverter.CanConvert(property.PropertyType);
        }
    }
}
