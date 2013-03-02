﻿using System;
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
        ConcurrentDictionary<Type, ConverterMap> ParsedMaps { get; set; }

        public DefaultUrlQueryMapper()
        {
            ParsedMaps = new ConcurrentDictionary<Type, ConverterMap>();
        }

        public CustomNameValueCollection Map(object model, object client)
        {
            return model == null 
                ? new CustomNameValueCollection()
                : ToDictionary(model, ParsedMaps.GetOrAdd(model.GetType(), x => new ConverterMap(x)));
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
            public IList<IConvertProperty> Converters { get; private set; }

            public ConverterMap(Type type)
            {
                Converters = Parse(type);
            }

            static IList<IConvertProperty> Parse(Type type)
            {
                return type.IsSimpleType()
                    ? new List<IConvertProperty>()
                    : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
            }

            static IConvertProperty ParseProperty(PropertyInfo info)
            {
                if (!info.IsUrlQuery())
                    return null;

                return TryGetCustomPropertyParser(info)
                    ?? ConvertCollectionProperty.TryCreate(info)
                    ?? ConvertSimpleProperty.TryCreate(info)
                    ?? ConvertObjectProperty.TryCreate(info);
            }

            static IConvertProperty TryGetCustomPropertyParser(PropertyInfo info)
            {
                var attribute = info.GetCustomAttribute<QueryPropertyConverterAttribute>(true);

                return attribute != null
                    ? attribute.GetConverter(info)
                    : null;
            }
        }
    }
}
