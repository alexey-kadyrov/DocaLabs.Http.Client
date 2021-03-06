﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts enumerable type properties.
    /// </summary>
    public class SimpleCollectionPropertyConverter : IPropertyConverter
    {
        readonly PropertyInfo _property;
        readonly IValueConverter _valueConverter;

        SimpleCollectionPropertyConverter(PropertyInfo property)
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

            _valueConverter = new SimpleCollectionValueConverter(name, format, culture);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new SimpleCollectionPropertyConverter(property)
                : null;
        }

        /// <summary>
        /// Converts a property value. If the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">Ignored.</param>
        /// <returns>One key-values pair.</returns>
        public ICustomKeyValueCollection Convert(object instance, ISet<object> processed)
        {
            return instance == null
                ? new CustomKeyValueCollection()
                : _valueConverter.Convert(_property.GetValue(instance));
        }

        static bool CanConvert(PropertyInfo property)
        {
            return !property.IsIndexer() && property.GetMethod != null && SimpleCollectionValueConverter.CanConvert(property.PropertyType);
        }
    }
}
