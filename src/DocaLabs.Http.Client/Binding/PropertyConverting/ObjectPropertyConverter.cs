using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class ObjectPropertyConverter : PropertyConverterBase, IPropertyConverter
    {
        ObjectPropertyConverter(PropertyInfo property, IPropertyConverterOverrides overrides)
            : base(property, overrides)
        {
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is not simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return property.PropertyType.IsSimpleType() || property.GetIndexParameters().Length > 0
                ? null
                : new ObjectPropertyConverter(property, overrides);
        }

        /// <summary>
        /// Serializes the property value to the string.
        /// If the object implements ICustomQueryMapper it will use ToParameterDictionary method to serialize the property,
        /// otherwise it will use ToString method.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-value pair.</returns>
        public CustomNameValueCollection Convert(object obj)
        {
            CustomNameValueCollection values = null;

            if (obj != null)
            {
                var value = Property.GetValue(obj, null);

                if (value != null)
                {
                    var customeMapper = /*ModelBinders.GetUrlQueryComposer(obj.GetType());

                    values = */ new CustomNameValueCollection { { Name, value.ToString() } };
                }
            }

            return values ?? new CustomNameValueCollection();
        }
    }
}
