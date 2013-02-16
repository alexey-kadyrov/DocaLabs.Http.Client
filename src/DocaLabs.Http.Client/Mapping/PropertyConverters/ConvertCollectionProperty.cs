using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Conversion;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    public class ConvertCollectionProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertCollectionProperty(PropertyInfo info)
            : base(info)
        {
        }

        public static IConvertProperty TryParse(PropertyInfo info)
        {
            return IsEnumerable(info) && GetEnumerableElementType(info).IsPrimitive
                ? new ConvertCollectionProperty(info)
                : null;
        }

        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            var collection = Info.GetValue(obj, null) as IEnumerable;

            if (collection != null)
            {
                foreach (var value in collection)
                {
                    values.Add(Name, ConvertValue(value));
                }
            }

            return values;
        }

        static bool IsEnumerable(PropertyInfo info)
        {
            var type = info.PropertyType;

            if (type == typeof(string) || type == typeof(byte[]))
                return false;

            return type.GetInterfaces().FirstOrDefault(x => x == typeof(IEnumerable)) != null;
        }

        static Type GetEnumerableElementType(PropertyInfo info)
        {
            var type = info.PropertyType;
            if (type.IsArray)
                return type.GetElementType();

            var genericEnumerable = type.GetInterfaces().FirstOrDefault(x => x == typeof(IEnumerable<>));
            return genericEnumerable != null
                ? genericEnumerable.GetGenericArguments()[0]
                : typeof(object);
        }
    }
}
