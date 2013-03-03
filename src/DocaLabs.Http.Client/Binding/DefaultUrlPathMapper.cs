using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Conversion;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultUrlPathMapper : IUrlPathMapper
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _parsedMaps = new ConcurrentDictionary<Type, PropertyMap>();

        public object[] Map(object model, object client)
        {
            return model == null 
                ? new object[0]
                : ToOrderedCollection(model, _parsedMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)));
        }

        static object[] ToOrderedCollection(object obj, PropertyMap map)
        {
            var values = new ArrayList();

            foreach (var converter in map.Converters)
            {
                values.Add(converter.ConvertValue(obj));
            }

            return values.ToArray();
        }

        class PropertyMap
        {
            public IList<PropertyConverter> Converters { get; private set; }

            public PropertyMap(Type type)
            {
                Converters = Parse(type);
            }

            static IList<PropertyConverter> Parse(Type type)
            {
                if (type.IsSimpleType())
                    return new List<PropertyConverter>();

                // thanks http://code.google.com/p/lokad-cloud/ for the idea

                var orderedCollection = type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttributes(typeof(QueryPathAttribute), true).Any())
                    // ordering always respect inheritance
                    .GroupBy(f => f.DeclaringType)
                        .OrderBy(g => g.Key, new InheritanceComparer())
                        .Select(g => g.OrderBy(f => f.GetCustomAttribute<QueryPathAttribute>(true).Order))
                    .SelectMany(f => f)
                    .ToList();

                return orderedCollection.Count == 0
                    ? new List<PropertyConverter>()
                    : orderedCollection.Select(ParseProperty).Where(x => x != null).ToList();
            }

            static PropertyConverter ParseProperty(PropertyInfo info)
            {
                if (info.IsUrlPath() && info.PropertyType.IsSimpleType())
                    return new PropertyConverter(info);

                return null;
            }

            class InheritanceComparer : IComparer<Type>
            {
                public int Compare(Type x, Type y)
                {
                    return ReferenceEquals(x, y) ? 0 : (x.IsSubclassOf(y) ? 1 : -1);
                }
            }
        }
        
        class PropertyConverter
        {
            PropertyInfo Info { get; set; }

            string Format { get; set; }

            public PropertyConverter(PropertyInfo info)
            {
                Info = info;
            }

            public string ConvertValue(object value)
            {
                if (value == null)
                    return string.Empty;

                return string.IsNullOrWhiteSpace(Format)
                           ? CustomConverter.Current.ChangeType<string>(value)
                           : string.Format("{0:" + Format + "}", value);
            }
        }
    }
}
