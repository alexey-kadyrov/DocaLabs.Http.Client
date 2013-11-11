using System;
using System.Globalization;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Specifies additional information about a property for converting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class PropertyOverridesAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a name that overrides the property name which is used by default.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the custom format string that is used by a property converter.
        /// If the format is non white space string then the converter will use string.Format
        /// to convert the property value. If set the format must include curly brackets.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets what culture should be used when the Format is non empty string.
        /// The default value is UseInvariant.
        /// </summary>
        public FormatCulture FormatCulture { get; set; }

        /// <summary>
        /// Returns instance of the CultureInfo class based on the FormatCulture property.
        /// </summary>
        public CultureInfo GetFormatCultureInfo()
        {
            switch (FormatCulture)
            {
                case FormatCulture.UseCurrent:
                    return CultureInfo.CurrentCulture;
                case FormatCulture.UseCurrentUI:
                    return CultureInfo.CurrentUICulture;
                default:
                    return CultureInfo.InvariantCulture;
            }
        }
    }
}