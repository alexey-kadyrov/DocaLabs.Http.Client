using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Converts simple properties, like int, string, Guid, etc.
    /// </summary>
    public class CustomNameValueCollectionPropertyConverter : PropertyConverterBase, IPropertyConverter 
    {
        CustomNameValueCollectionPropertyConverter(PropertyInfo property, INamedPropertyConverterInfo info)
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

            return typeof (CustomNameValueCollection).IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0
                       ? new CustomNameValueCollectionPropertyConverter(property, info) 
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
                var value = Property.GetValue(obj, null) as CustomNameValueCollection;

                if (value != null)
                {
                    values.AddRange(value);
                }
            }

            return values;
        }
    }
}