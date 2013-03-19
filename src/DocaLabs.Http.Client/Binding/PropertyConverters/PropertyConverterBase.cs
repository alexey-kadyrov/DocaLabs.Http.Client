using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
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
        /// Gets the name which should be used. If RequestQueryAttribute is not defined or the Name in the attribute
        /// is null or blank then this property will set to the linked property name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// gets the custom format string that is set by RequestQueryAttribute.
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// Initializes Property, Name and Format properties.
        /// </summary>
        protected PropertyConverterBase(PropertyInfo property, INamedPropertyConverterInfo info)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            Property = property;

            if(info != null)
            {
                Name = info.Name;
                Format = string.IsNullOrWhiteSpace(Format)
                    ? null
                    : "{0:" + info.Format + "}";
            }

            if(string.IsNullOrWhiteSpace(Name))
                Name = Property.Name;
        }

        /// <summary>
        /// Converts a single to its string representation, if the Format is specified then string.Format is used.
        /// </summary>
        public string ConvertValue(object value)
        {
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(Format)
                       ? CustomConverter.Current.ChangeType<string>(value)
                       : string.Format(Format, value);
        }
    }
}
