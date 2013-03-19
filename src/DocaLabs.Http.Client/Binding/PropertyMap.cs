using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    abstract class PropertyMap
    {
        public IList<IPropertyConverter> Converters { get; private set; }

        protected PropertyMap(Type type)
        {
            Converters = Parse(type);
        }

        public CustomNameValueCollection ConvertModel(object obj)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in Converters)
                values.AddRange(converter.GetValue(obj));

            return values;
        }

        IList<IPropertyConverter> Parse(Type type)
        {
            return type.IsSimpleType()
                ? new List<IPropertyConverter>()
                : type.GetAllInstancePublicProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        IPropertyConverter ParseProperty(PropertyInfo property)
        {
            if (!AcceptProperty(property))
                return null;

            var converterInfo = GetConverterInfo(property);

            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property, converterInfo)
                ?? CustomNameValueCollectionPropertyConverter.TryCreate(property, converterInfo)
                ?? NameValueCollectionPropertyConverter.TryCreate(property, converterInfo)
                ?? CollectionPropertyConverter.TryCreate(property, converterInfo)
                ?? ObjectPropertyConverter.TryCreate(property, converterInfo);
        }

        protected abstract bool AcceptProperty(PropertyInfo property);

        protected virtual INamedPropertyConverterInfo GetConverterInfo(PropertyInfo property)
        {
            return null;
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