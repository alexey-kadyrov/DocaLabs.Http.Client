﻿using System;
using System.Collections;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Converts enumerable of simple type properties.
    /// </summary>
    public class CollectionPropertyConverter : PropertyConverterBase<RequestQueryAttribute>, IPropertyConverter
    {
        CollectionPropertyConverter(PropertyInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <param name="info">Property for which instance of the CollectionPropertyConverter should be created.</param>
        /// <returns>Instance of the CollectionPropertyConverter class if the info describes the enumerable of simple types otherwise null.</returns>
        public static IPropertyConverter TryCreate(PropertyInfo info)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            var type = info.PropertyType;

            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && info.GetIndexParameters().Length == 0
                ? new CollectionPropertyConverter(info)
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
                var collection = Info.GetValue(obj, null) as IEnumerable;

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