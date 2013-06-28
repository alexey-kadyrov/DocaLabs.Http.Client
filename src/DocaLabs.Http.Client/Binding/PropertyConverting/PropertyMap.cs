using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public abstract class PropertyMap
    {
        readonly IList<IPropertyConverter> _converters;
        readonly Func<object, PropertyMap> _propertyMapGetOrAddType;

        protected PropertyMap(Type type, Func<object, PropertyMap> propertyMapGetOrAddType)
        {
            _propertyMapGetOrAddType = propertyMapGetOrAddType;
            _converters = Parse(type);
        }

        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            foreach (var converter in _converters)
                values.Add(converter.Convert(obj));

            return values;
        }

        IList<IPropertyConverter> Parse(Type type)
        {
            return type.IsSimpleType()
                ? new List<IPropertyConverter>()
                : type.GetAllPublicInstanceProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        IPropertyConverter ParseProperty(PropertyInfo property)
        {
            return AcceptProperty(property)
                ? GetConverter(property, GetPropertyConverterOverrides(property)) 
                : null;
        }

        protected abstract bool AcceptProperty(PropertyInfo property);

        protected virtual IPropertyConverterOverrides GetPropertyConverterOverrides(PropertyInfo property)
        {
            return null;
        }

        IPropertyConverter GetConverter(PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property, overrides)
                ?? NameValueCollectionPropertyConverter.TryCreate(property, overrides)
                ?? CollectionPropertyConverter.TryCreate(property, overrides)
                ?? ObjectPropertyConverter.TryCreate(property, overrides, _propertyMapGetOrAddType);
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