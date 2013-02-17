using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Converts simple properties, like int, string, Guid, etc.
    /// </summary>
    public class ConvertSimpleProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertSimpleProperty(PropertyInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <param name="info">Property for which instance of the ConvertSimpleProperty should be created.</param>
        /// <returns>Instance of the ConvertSimpleProperty class if the info describes the simple property otherwise null.</returns>
        public static IConvertProperty TryCreate(PropertyInfo info)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            return info.PropertyType.IsSimpleType() && info.GetIndexParameters().Length == 0
                ? new ConvertSimpleProperty(info) 
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
                var value = Info.GetValue(obj, null);

                if (value != null)
                    values.Add(Name, ConvertValue(value));
            }

            return values;
        }
    }
}
