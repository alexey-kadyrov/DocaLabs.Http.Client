using System;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to get a custom converter that should be used to convert a property into name-value pairs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class CustomPropertyConverterAttribute : Attribute
    {
        /// <summary>
        /// Returns converter for a property.
        /// </summary>
        /// <param name="info">IConverter for which the attribute was set.</param>
        /// <returns>Class implementing IConverter that can be used to convert the property value.</returns>
        public abstract IConverter GetConverter(PropertyInfo info);
    }
}