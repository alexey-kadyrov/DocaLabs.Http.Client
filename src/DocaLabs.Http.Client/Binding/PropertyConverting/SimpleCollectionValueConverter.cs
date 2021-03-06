﻿using System;
using System.Collections;
using System.Globalization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts enumerable type values.
    /// </summary>
    public class SimpleCollectionValueConverter : IValueConverter
    {
        readonly string _name;
        readonly string _format;
        readonly CultureInfo _culture;

        /// <summary>
        /// Initializes an instance of the SimpleCollectionValueConverter class.
        /// </summary>
        /// <param name="name">Name which should be used as the key for the converted values.</param>
        /// <param name="format">If the format is non empty then string.Format is used for converting.</param>
        /// <param name="culture">Culture to be used if the format is non empty string.</param>
        public SimpleCollectionValueConverter(string name, string format, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _name = name;
            _format = format;
            _culture = culture;
        }

        /// <summary>
        /// Converts a collection value. If the value is null then the return collection will be empty.
        /// </summary>
        /// <param name="value">The collection.</param>
        /// <returns>One key-values pair.</returns>
        public ICustomKeyValueCollection Convert(object value)
        {
            var values = new CustomKeyValueCollection();

            var collection = value as IEnumerable;

            if (collection != null)
            {
                foreach (var item in collection)
                {
                    if(item != null && !item.GetType().IsSimpleType())
                        continue;

                    values.Add(_name, CustomConverter.ChangeToString(_format, _culture, item));
                }
            }

            return values;
        }

        /// <summary>
        /// Returns whenever the type can be converted by the SimpleCollectionValueConverter.
        /// </summary>
        /// <returns>True if the type is enumerable of simple types, like int, double, string, byte[], DateTime.</returns>
        public static bool CanConvert(Type type)
        {
            if (!type.IsEnumerable())
                return false;

            var elementType = type.GetEnumerableElementType();

            return elementType == typeof(object) || type.GetEnumerableElementType().IsSimpleType();
        }
    }
}
