using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    class QueryPropertyMap
    {
        public IList<IPropertyConverter> Converters { get; private set; }

        public QueryPropertyMap(Type type)
        {
            Converters = Parse(type);
        }

        static IList<IPropertyConverter> Parse(Type type)
        {
            return type.IsSimpleType()
                       ? new List<IPropertyConverter>()
                       : type.GetAllInstancePublicProperties()
                             .Select(ParseProperty)
                             .Where(x => x != null)
                             .ToList();
        }

        static IPropertyConverter ParseProperty(PropertyInfo info)
        {
            if (!info.IsExplicitUrlQuery())
                return null;

            return TryGetCustomPropertyParser(info)
                   ?? CollectionPropertyConverter.TryCreate(info)
                   ?? SimplePropertyConverter<RequestQueryAttribute>.TryCreate(info)
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