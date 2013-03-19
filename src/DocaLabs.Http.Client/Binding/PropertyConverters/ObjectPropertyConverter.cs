using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class ObjectPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        ObjectPropertyConverter(PropertyInfo property, INamedPropertyConverterInfo info)
            : base(property, info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <returns>Instance of the ObjectPropertyConverter class if the info describes the reference type property otherwise null.</returns>
        public static IPropertyConverter TryCreate(PropertyInfo property, INamedPropertyConverterInfo info)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return property.PropertyType.IsSimpleType() || property.GetIndexParameters().Length > 0
                ? null
                : new ObjectPropertyConverter(property, info);
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
                var value = Property.GetValue(obj, null);

                if (value != null)
                {
                    var customeMapper = /*ClientModelBinders.GetUrlQueryComposer(obj.GetType());

                    values = */ new CustomNameValueCollection { { Name, value.ToString() } };
                }
            }

            return values ?? new CustomNameValueCollection();
        }
    }
}
