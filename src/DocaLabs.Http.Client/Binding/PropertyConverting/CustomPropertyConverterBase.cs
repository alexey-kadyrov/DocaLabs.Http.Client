using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Helper base class for custom property converters.
    /// </summary>
    public abstract class CustomPropertyConverterBase : IPropertyConverter
    {
        protected PropertyInfo PropertyInfo { get; private set; }

        protected CustomPropertyConverterBase(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        /// <summary>
        /// Converts a property value.
        /// Bear in mind that the keys will be used as they are returned from the method without any possible concatination for nested values.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">List of object (values which are not int, string, etc.) that were processed in order to prevent circular references.</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        public NameValueCollection Convert(object instance, ISet<object> processed)
        {
            return Convert(PropertyInfo.GetValue(instance));
        }

        /// <summary>
        /// Converts a property value.
        /// </summary>
        /// <param name="value">The value of the property</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        public abstract NameValueCollection Convert(object value);
    }
}
