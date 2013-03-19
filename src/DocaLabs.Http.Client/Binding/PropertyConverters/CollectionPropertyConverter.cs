﻿using System;
using System.Collections;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Converts enumerable of simple type properties.
    /// </summary>
    public class CollectionPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        CollectionPropertyConverter(PropertyInfo property, INamedPropertyConverterInfo info)
            : base(property, info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <returns>Instance of the CollectionPropertyConverter class if the info describes the enumerable of simple types otherwise null.</returns>
        public static IPropertyConverter TryCreate(PropertyInfo property, INamedPropertyConverterInfo info)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0
                ? new CollectionPropertyConverter(property, info)
                : null;
        }

        /// <summary>
        /// Serializes the property value to the string.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-values pair.</returns>
        public CustomNameValueCollection GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
            {
                var collection = Property.GetValue(obj, null) as IEnumerable;

                if (collection != null)
                {
                    foreach (var value in collection)
                    {
                        values.Add(Name, ConvertValue(value));
                    }
                }
            }

            return values;
        }
    }
}
