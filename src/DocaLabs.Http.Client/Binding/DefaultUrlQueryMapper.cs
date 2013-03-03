using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultUrlQueryMapper : IUrlQueryMapper
    {
        readonly ConcurrentDictionary<Type, ConverterMap> _parsedMaps = new ConcurrentDictionary<Type, ConverterMap>();

        public CustomNameValueCollection Map(object model, object client)
        {
            return model == null 
                ? new CustomNameValueCollection()
                : ToDictionary(model, _parsedMaps.GetOrAdd(model.GetType(), x => new ConverterMap(x)));
        }

        static CustomNameValueCollection ToDictionary(object obj, ConverterMap map)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in map.Converters)
                values.AddRange(converter.GetValue(obj));

            return values;
        }

        class ConverterMap
        {
            public IList<IPropertyConverter> Converters { get; private set; }

            public ConverterMap(Type type)
            {
                Converters = Parse(type);
            }

            static IList<IPropertyConverter> Parse(Type type)
            {
                return type.IsSimpleType()
                    ? new List<IPropertyConverter>()
                    : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
            }

            static IPropertyConverter ParseProperty(PropertyInfo info)
            {
                if (!info.IsUrlQuery())
                    return null;

                return TryGetCustomPropertyParser(info)
                    ?? CollectionPropertyConverter.TryCreate(info)
                    ?? SimplePropertyConverter.TryCreate(info)
                    ?? ObjectPropertyConverter.TryCreate(info);
            }

            static IPropertyConverter TryGetCustomPropertyParser(PropertyInfo info)
            {
                var attribute = info.GetCustomAttribute<QueryPropertyConverterAttribute>(true);

                return attribute != null
                    ? attribute.GetConverter(info)
                    : null;
            }
        }
    }
}
