﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts NameValueCollection type properties.
    /// </summary>
    public class NameValueCollectionPropertyConverter : IPropertyConverter 
    {
        readonly PropertyInfo _property;
        readonly IValueConverter _valueConverter;

        NameValueCollectionPropertyConverter(PropertyInfo property)
        {
            _property = property;

            string name = null, format = null;

            var requestUse = property.GetCustomAttribute<RequestUseAttribute>();
            if (requestUse != null)
            {
                name = requestUse.Name;
                format = requestUse.Format;
            }

            if (name == null)
                name = _property.Name;

            _valueConverter = new SimpleValueConverter(name, format);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is derived from NameValueCollection
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new NameValueCollectionPropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the instance is null or the value of the property is null then the return collection will be empty.
        /// If the Name was overridden (The IsOverridden is true) then it will be added to the key from the collection,
        /// e.g. key = Name + "." + itemKey
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">Ignored.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object instance, ISet<object> processed)
        {
            return instance == null
                ? new NameValueCollection()
                : _valueConverter.Convert(_property.GetValue(instance));
        }

        static bool CanConvert(PropertyInfo property)
        {
            return typeof(NameValueCollection).IsAssignableFrom(property.PropertyType) && !property.IsIndexer();
        }
    }
}