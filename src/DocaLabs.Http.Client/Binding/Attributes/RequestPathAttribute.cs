using System;

namespace DocaLabs.Http.Client.Binding.Attributes
{
    /// <summary>
    /// Base class to mark a property for mapping to an URL's path part.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class RequestPathAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a format string that should be used when converting the property value.
        /// </summary>
        public string Format { get; set; }
    }
}
