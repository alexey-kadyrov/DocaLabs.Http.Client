using System;
using System.Reflection;
using DocaLabs.Conversion;
using DocaLabs.Http.Client.Mapping.Attributes;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Base class for property converters.
    /// </summary>
    public abstract class PropertyConverterBase
    {
        /// <summary>
        /// Gets the linked property info.
        /// </summary>
        protected PropertyInfo Info { get; private set; }

        /// <summary>
        /// Gets the name which should be used. If QueryParameterAttribute is not defined or the Name in the attribute
        /// is null or blank then this property will set to the linked property name.
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// gets the custom format string that is set by QueryParameterAttribute.
        /// </summary>
        protected string Format { get; private set; }

        /// <summary>
        /// Initializes Info, Name and Format properties.
        /// </summary>
        protected PropertyConverterBase(PropertyInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Info = info;

            var attribute = info.GetCustomAttribute<QueryParameterAttribute>(true);
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

        protected string ConvertValue(object value)
        {
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(Format)
                       ? CustomConverter.Current.ChangeType<string>(value)
                       : string.Format("{0:" + Format + "}", value);
        }
    }
}
