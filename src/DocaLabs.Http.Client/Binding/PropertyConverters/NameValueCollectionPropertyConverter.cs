using System;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Converts simple properties, like int, string, Guid, etc.
    /// </summary>
    public class NameValueCollectionPropertyConverter : PropertyConverterBase, IPropertyConverter 
    {
        NameValueCollectionPropertyConverter(PropertyInfo property, INamedPropertyConverterInfo info)
            : base(property, info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <returns>Instance of the SimplePropertyConverter class if the info describes the simple property otherwise null.</returns>
        public static IPropertyConverter TryCreate(PropertyInfo property, INamedPropertyConverterInfo info)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return typeof (NameValueCollection).IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0
                       ? new NameValueCollectionPropertyConverter(property, info) 
                       : null;
        }

        /// <summary>
        /// Serializes the property value to the string.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-value pair.</returns>
        public CustomNameValueCollection GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
            {
                var value = Property.GetValue(obj, null) as NameValueCollection;

                if (value != null)
                {
                    foreach (var key in value.AllKeys)
                    {
                        values.Add(key, value[key]);
                    }
                }
            }

            return values;
        }
    }
}