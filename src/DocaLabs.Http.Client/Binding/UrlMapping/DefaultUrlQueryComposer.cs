using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class DefaultUrlQueryComposer : IUrlQueryComposer
    {
        readonly ConcurrentDictionary<Type, ConverterMap> _converterMaps = new ConcurrentDictionary<Type, ConverterMap>();

        public string Compose(object model, Uri baseUrl)
        {
            var existingQuery = GetExistingQuery(baseUrl);
            if (Ignore(model))
                return existingQuery;

            var modelQuery = ConvertModelToQuery(model, _converterMaps.GetOrAdd(model.GetType(), x => new ConverterMap(x)));

            if (string.IsNullOrWhiteSpace(existingQuery))
                return modelQuery;

            return string.IsNullOrWhiteSpace(modelQuery)
                ? existingQuery
                : ConcatenateQueryParts(existingQuery, modelQuery);
        }

        static bool Ignore(object model)
        {
            return model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        static string GetExistingQuery(Uri baseUrl)
        {
            var query = baseUrl == null ? "" : baseUrl.Query;

            return GetQueryWithoutQuestionMark(query);
        }

        static string GetQueryWithoutQuestionMark(string query)
        {
            return query.StartsWith("?")
                       ? query.Substring(1)
                       : query;
        }

        static string ConvertModelToQuery(object obj, ConverterMap map)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in map.Converters)
                values.AddRange(converter.GetValue(obj));

            return new QueryStringBuilder().Add(values).ToString();
        }

        static string ConcatenateQueryParts(string leftPart, string rightPart)
        {
            return leftPart.EndsWith("&")
                    ? leftPart + rightPart
                    : leftPart + "&" + rightPart;
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
                var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

                return attribute != null
                    ? attribute.GetConverter(info)
                    : null;
            }
        }
    }
}
