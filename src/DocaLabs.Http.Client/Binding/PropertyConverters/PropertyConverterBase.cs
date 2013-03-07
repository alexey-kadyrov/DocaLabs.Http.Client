﻿using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Base class for property converters.
    /// </summary>
    public abstract class PropertyConverterBase<T>
        where T : Attribute, INamedPropertyConverterInfo
    {
        /// <summary>
        /// Gets the linked property info.
        /// </summary>
        protected PropertyInfo Info { get; private set; }

        /// <summary>
        /// Gets the name which should be used. If RequestQueryAttribute is not defined or the Name in the attribute
        /// is null or blank then this property will set to the linked property name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// gets the custom format string that is set by RequestQueryAttribute.
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// Initializes Info, Name and Format properties.
        /// </summary>
        protected PropertyConverterBase(PropertyInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Info = info;

            var attribute = info.GetCustomAttribute<T>(true);
            if(attribute != null)
            {
                Name = attribute.Name;
                Format = attribute.Format;
            }

            if(string.IsNullOrWhiteSpace(Name))
            {
                Name = Info.Name;
            }
        }

        /// <summary>
        /// Converts a single to its string representation, if the Format is specified then string.Format is used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertValue(object value)
        {
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(Format)
                       ? CustomConverter.Current.ChangeType<string>(value)
                       : string.Format("{0:" + Format + "}", value);
        }
    }
}
