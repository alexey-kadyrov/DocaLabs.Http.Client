using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public class PropertyMap : IPropertyConverter
    {
        readonly ConcurrentDictionary<Type, IPropertyConverter> _nestedConverters;
        readonly IList<IPropertyConverter> _converters;
        readonly Func<PropertyInfo, bool> _acceptPropertyCheck;

        public PropertyMap(Type type, Func<PropertyInfo, bool> acceptPropertyCheck)
            : this(type, acceptPropertyCheck, new ConcurrentDictionary<Type, IPropertyConverter>())
        {
        }

        public PropertyMap(Type type, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, IPropertyConverter> nestedConverters)
        {
            _nestedConverters = nestedConverters;
            _acceptPropertyCheck = acceptPropertyCheck;
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
            _nestedConverters[type] = this;

            return type.IsSimpleType()
                ? new List<IPropertyConverter>()
                : type.GetAllPublicInstanceProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        IPropertyConverter ParseProperty(PropertyInfo property)
        {
            return _acceptPropertyCheck(property)
                ? GetConverter(property) 
                : null;
        }

        IPropertyConverter GetConverter(PropertyInfo property)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property)
                ?? NameValueCollectionPropertyConverter.TryCreate(property)
                ?? SimpleCollectionPropertyConverter.TryCreate(property)
                ?? NestedTypesPropertyConverter.TryCreate(property, _acceptPropertyCheck, _nestedConverters);
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