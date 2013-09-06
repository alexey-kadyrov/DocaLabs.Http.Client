using System;
using System.Reflection;
using DocaLabs.Http.Client.RequestSerialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class ConvertObjectProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertObjectProperty(PropertyInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <param name="info">Property for which instance of the ConvertObjectProperty should be created.</param>
        /// <returns>Instance of the ConvertObjectProperty class if the info describes the reference type property otherwise null.</returns>
        public static IConvertProperty TryCreate(PropertyInfo info)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            return info.PropertyType.IsSimpleType() || info.GetIndexParameters().Length > 0
                ? null
                : new ConvertObjectProperty(info);
        }

        /// <summary>
        /// Serializes the property value to the string.
        /// If the object implements ICustomQueryMapper it will use ToParameterDictionary method to serialize the property,
        /// otherwise it will use ToString method.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-value pair.</returns>
        public CustomNameValueCollection GetValue(object obj)
        {
            CustomNameValueCollection values = null;

            if (obj != null)
            {
                var value = Info.GetValue(obj, null);

                if (value != null)
                {
                    var customeMapper = value as ICustomQueryMapper;

                    if (customeMapper != null)
                    {
                        values = customeMapper.ToParameterDictionary();
                    }
                    else
                    {
                        if(Info.GetCustomAttributes(typeof(RequestSerializationAttribute), true).Length == 0)
                            values = new CustomNameValueCollection { { Name, value.ToString() } };
                    }
                }
            }

            return values ?? new CustomNameValueCollection();
        }
    }
}
