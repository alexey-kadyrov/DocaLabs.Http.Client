using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Specifies additional information about a property for serializing into a URI's query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InRequestQueryAttribute : Attribute, IPropertyConverterOverrides
    {
        /// <summary>
        /// Gets or sets a name that should be used for the property in a URI's query.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a format string that should be used when converting the property value.
        /// </summary>
        public string Format { get; set; }
    }
}
