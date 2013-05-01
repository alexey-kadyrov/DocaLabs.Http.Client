using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InRequestHeaderAttribute : Attribute, IPropertyConverterOverrides
    {
        /// <summary>
        /// Gets or sets a name that should be used for the request's header.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets the custom format string that is set by InRequestQueryAttribute.
        /// </summary>
        public string Format { get; set; }
    }
}
