using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public class TypeConverter : IConverter
    {
        readonly Func<PropertyInfo, bool> _acceptPropertyCheck;
        readonly IList<IConverter> _converters;
        readonly ConcurrentDictionary<Type, TypeConverter> _processedTypes;

        public TypeConverter(Type type, Func<PropertyInfo, bool> acceptPropertyCheck)
            : this(type, acceptPropertyCheck, new ConcurrentDictionary<Type, TypeConverter>())
        {
        }

        public TypeConverter(Type type, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, TypeConverter> processedTypes)
        {
            _processedTypes = processedTypes;
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

        IList<IConverter> Parse(Type type)
        {
            _processedTypes[type] = this;

            return type.IsSimpleType()
                ? new List<IConverter>()
                : type.GetAllPublicInstanceProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        IConverter ParseProperty(PropertyInfo property)
        {
            return _acceptPropertyCheck(property)
                ? GetConverter(property) 
                : null;
        }

        IConverter GetConverter(PropertyInfo property)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property)
                ?? NameValueCollectionPropertyConverter.TryCreate(property)
                ?? SimpleCollectionPropertyConverter.TryCreate(property)
                ?? ObjectPropertyConverter.TryCreate(property, _acceptPropertyCheck, _processedTypes);
        }

        static IConverter TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }
    }
}