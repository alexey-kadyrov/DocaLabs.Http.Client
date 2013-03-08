using System;
using DocaLabs.Http.Client.Binding.PropertyConverters;

namespace DocaLabs.Http.Client.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class RequestHeaderAttribute : Attribute, INamedPropertyConverterInfo
    {
        /// <summary>
        /// Gets or sets a name that should be used for the request's header.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets the custom format string that is set by RequestQueryAttribute.
        /// </summary>
        public string Format { get; set; }
    }
}
