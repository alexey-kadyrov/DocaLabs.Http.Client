using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Mapping.Attributes;
using DocaLabs.Http.Client.Binding.Mapping.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultUrlMapper : IUrlMapper
    {
        ConcurrentDictionary<Type, TypeMap> ParsedTypeMaps { get; set; }

        public DefaultUrlMapper()
        {
            ParsedTypeMaps = new ConcurrentDictionary<Type, TypeMap>();
        }

        public CustomNameValueCollection Map(RequestContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            return context.QueryModel == null 
                ? new CustomNameValueCollection() 
                : ToDictionary(context.QueryModel, ParsedTypeMaps.GetOrAdd(context.QueryModel.GetType(), x => new TypeMap(x)));
        }

        static CustomNameValueCollection ToDictionary(object obj, TypeMap map)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in map.Converters)
                values.AddRange(converter.GetValue(obj));

            return values;
        }

        class TypeMap
        {
            public IList<IConvertProperty> Converters { get; private set; }

            public TypeMap(Type type)
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
                if (Ignore(info))
                    return null;

                return TryGetCustomPropertyParser(info)
                    ?? ConvertCollectionProperty.TryCreate(info)
                    ?? ConvertSimpleProperty.TryCreate(info)
                    ?? ConvertObjectProperty.TryCreate(info);
            }

            static bool Ignore(PropertyInfo info)
            {
                // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
                return info.GetIndexParameters().Length > 0 ||
                        info.GetGetMethod() == null ||
                        info.GetCustomAttribute<QueryIgnoreAttribute>(true) != null;
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
