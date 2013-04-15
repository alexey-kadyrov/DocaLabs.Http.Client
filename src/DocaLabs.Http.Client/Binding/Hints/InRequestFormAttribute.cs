using System;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client.Binding.Hints
{
    /// <summary>
    /// Specifies additional information about a property for serializing into a URI's query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InRequestFormAttribute : Attribute, IPropertyConverterOverrides
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