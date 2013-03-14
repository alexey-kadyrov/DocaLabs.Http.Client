using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    abstract class QueryPropertyMapBase
    {
        protected QueryPropertyMapBase(Type type)
        {
            Converters = Parse(type);
        }

        public IList<IPropertyConverter> Converters { get; private set; }

        IList<IPropertyConverter> Parse(Type type)
        {
            return type.IsSimpleType()
                       ? new List<IPropertyConverter>()
                       : type.GetAllInstancePublicProperties()
                             .Select(ParseProperty)
                             .Where(x => x != null)
                             .ToList();
        }

        IPropertyConverter ParseProperty(PropertyInfo info)
        {
            if (!IsSuitableForUrlQuery(info))
                return null;

            return TryGetCustomPropertyParser(info)
                   ?? CollectionPropertyConverter<RequestQueryAttribute>.TryCreate(info)
                   ?? SimplePropertyConverter<RequestQueryAttribute>.TryCreate(info)
                   ?? ObjectPropertyConverter<RequestQueryAttribute>.TryCreate(info);
        }

        protected abstract bool IsSuitableForUrlQuery(PropertyInfo info);

        static IPropertyConverter TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }
    }
}