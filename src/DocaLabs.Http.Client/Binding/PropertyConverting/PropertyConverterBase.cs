﻿using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Base class for property converters.
    /// </summary>
    public abstract class PropertyConverterBase
    {
        /// <summary>
        /// Gets the linked property info.
        /// </summary>
        protected PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the name which should be used.
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// Gets the custom format string, if the format is non white space string then 
        /// the converter will use string.Format to convert the property value.
        /// </summary>
        protected string Format { get; private set; }

        /// <summary>
        /// Gets a flag whenever the property name was overridden by IPropertyConverterOverrides.
        /// </summary>
        protected bool IsNameOverridden { get; private set; }

        /// <summary>
        /// Initializes the instance of the PropertyConverterBase class for the provided property and optionally overrides.
        /// </summary>
        protected PropertyConverterBase(PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            Property = property;

            if (overrides != null)
            {
                Name = overrides.Name;
                Format = overrides.Format;
                IsNameOverridden = true;
            }

            if (!string.IsNullOrWhiteSpace(Name))
                return;

            Name = Property.Name;
            IsNameOverridden = false;
        }

        /// <summary>
        /// Helper method to convert a simple value to its string representation.
        /// If the value is null then empty string is returned.
        /// If the Format is specified then string.Format is used.
        /// </summary>
        protected string ConvertSimpleValue(object value)
        {
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(Format)
                ? CustomConverter.Current.ChangeType<string>(value)
                : string.Format(Format, value);
        }

        /// <summary>
        /// Helper method to get a key maker.
        /// </summary>
        protected Func<string, string> GetKeyMaker()
        {
            return IsNameOverridden
                ? (Func<string, string>)MakeCompositeName
                : GetNameAsIs;
        }

        static string GetNameAsIs(string name)
        {
            return name;
        }

        string MakeCompositeName(string name)
        {
            return Name + "." + name;
        }
    }
}