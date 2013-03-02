using System;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverters;

namespace DocaLabs.Http.Client.Binding.Attributes
{
    /// <summary>
    /// Defines methods for a custom converter that should be used to convert a property for serializing into URI's query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class QueryPropertyConverterAttribute : Attribute
    {
        /// <summary>
        /// Returns converter for a property.
        /// </summary>
        /// <param name="info">IPropertyConverter for which the attribute was set.</param>
        /// <returns>Class implementing IPropertyConverter that can be used to convert the property value.</returns>
        public abstract IPropertyConverter GetConverter(PropertyInfo info);
    }
}