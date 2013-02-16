using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Converts enumerable of simple type properties.
    /// </summary>
    public class ConvertCollectionProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertCollectionProperty(PropertyInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Tries to create the converter for the specified property.
        /// </summary>
        /// <param name="info">Property for which instance of the ConvertCollectionProperty should be created.</param>
        /// <returns>Instance of the ConvertCollectionProperty class if the info describes the enumerable of simple types otherwise null.</returns>
        public static IConvertProperty TryCreate(PropertyInfo info)
        {
            if(info == null)
                throw new ArgumentNullException("info");

            return IsEnumerable(info) && GetEnumerableElementType(info).IsSimpleType() && info.GetIndexParameters().Length == 0
                ? new ConvertCollectionProperty(info)
                : null;
        }

        /// <summary>
        /// Serializes the property value to the string.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-values pair.</returns>
        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
            {
                var collection = Info.GetValue(obj, null) as IEnumerable;

                if (collection != null)
                {
                    foreach (var value in collection)
                    {
                        values.Add(Name, ConvertValue(value));
                    }
                }
            }

            return values;
        }

        static bool IsEnumerable(PropertyInfo info)
        {
            var type = info.PropertyType;

            if (type == typeof(string) || type == typeof(byte[]))
                return false;

            return type == typeof(IEnumerable) || type.GetInterfaces().Any(x => x == typeof(IEnumerable));
        }

        static Type GetEnumerableElementType(PropertyInfo info)
        {
            var type = info.PropertyType;
            if (type.IsArray)
                return type.GetElementType();

            if (type == typeof (IEnumerable))
                return typeof (object);

            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                return type.GetGenericArguments()[0];

            var genericEnumerable = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return genericEnumerable != null
                ? genericEnumerable.GetGenericArguments()[0]
                : typeof(object);
        }
    }
}
