using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Manages property maps of parsed types and converts instances into NameValueCollection where keys/values correspond to property names/values.
    /// Converting is delegated to:
    ///     NameValueCollectionPropertyConverter
    ///     SimpleDictionaryPropertyConverter
    ///     SimpleCollectionPropertyConverter
    ///     SimplePropertyConverter
    /// It's able to convert a hierarchy of nested types with circular references.
    /// </summary>
    public class PropertyMaps
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _maps = new ConcurrentDictionary<Type, PropertyMap>();

        /// <summary>
        /// Delegate which is used to check whenever the passed property should be parsed.
        /// </summary>
        public Func<PropertyInfo, bool> AcceptPropertyCheck { get; private set; }

        /// <summary>
        /// Initializes an instance of the PropertyMaps class.
        /// </summary>
        public PropertyMaps(Func<PropertyInfo, bool> acceptPropertyCheck)
        {
            AcceptPropertyCheck = acceptPropertyCheck;
        }

        /// <summary>
        /// Converts instance into NameValueCollection where keys/values correspond to property names/values.
        /// </summary>
        public NameValueCollection Convert(object instance)
        {
            return Convert(instance, new HashSet<object>());
        }

        internal NameValueCollection Convert(object instance, ISet<object> processed)
        {
            if (instance == null)
                return new NameValueCollection();

            var type = instance.GetType();

            PropertyMap map;

            if (_maps.TryGetValue(type, out map))
                return map.Convert(instance, processed);

            map = new PropertyMap(this);

            _maps.TryAdd(type, map);

            map.Parse(instance);

            return map.Convert(instance, processed);
        }

        class PropertyMap
        {
            readonly PropertyMaps _maps;
            List<IPropertyConverter> _converters;

            internal PropertyMap(PropertyMaps maps)
            {
                _maps = maps;
            }

            internal void Parse(object instance)
            {
                var type = instance.GetType();

                _converters = SkipParsing(type)
                    ? new List<IPropertyConverter>()
                    : type.GetAllPublicInstanceProperties()
                            .Select(ParseProperty)
                            .Where(x => x != null)
                            .ToList();
            }

            internal NameValueCollection Convert(object instance, ISet<object> processed)
            {
                var type = instance.GetType();

                if (NameValueCollectionValueConverter.CanConvert(type))
                    return new NameValueCollectionValueConverter(null).Convert(instance);

                if (SimpleDictionaryValueConverter.CanConvert(type))
                    return new SimpleDictionaryValueConverter(null, null).Convert(instance);

                var values = new NameValueCollection();

                if (_converters == null || _converters.Count == 0)
                    return values;

                processed.Add(instance);

                foreach (var converter in _converters)
                    values.Add(converter.Convert(instance, processed));

                return values;
            }

            IPropertyConverter ParseProperty(PropertyInfo propertyInfo)
            {
                return _maps.AcceptPropertyCheck(propertyInfo)
                    ? GetConverter(propertyInfo)
                    : null;
            }

            IPropertyConverter GetConverter(PropertyInfo property)
            {
                return TryGetCustomPropertyParser(property)
                    ?? NameValueCollectionPropertyConverter.TryCreate(property)
                    ?? SimpleDictionaryPropertyConverter.TryCreate(property)
                    ?? SimpleCollectionPropertyConverter.TryCreate(property)
                    ?? SimplePropertyConverter.TryCreate(property)
                    ?? ObjectPropertyConverter.TryCreate(property, _maps);
            }

            static IPropertyConverter TryGetCustomPropertyParser(PropertyInfo property)
            {
                var customConverter = property.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

                return customConverter != null
                    ? customConverter.GetConverter(property)
                    : null;
            }

            static bool SkipParsing(Type type)
            {
                return type == typeof(object) ||
                    type.IsSimpleType() ||
                    NameValueCollectionValueConverter.CanConvert(type) ||
                    SimpleDictionaryValueConverter.CanConvert(type);
            }

            class ObjectPropertyConverter : IPropertyConverter
            {
                readonly PropertyMaps _maps;
                readonly PropertyInfo _property;
                readonly string _name;
                readonly string _format;

                ObjectPropertyConverter(PropertyInfo property, PropertyMaps maps)
                {
                    _property = property;
                    _maps = maps;

                    var requestUse = property.GetCustomAttribute<PropertyOverridesAttribute>();
                    if (requestUse != null)
                    {
                        _name = requestUse.Name;
                        _format = requestUse.Format;
                    }

                    if (_name == null)
                        _name = _property.Name;
                }

                public static IPropertyConverter TryCreate(PropertyInfo property, PropertyMaps maps)
                {
                    return CanConvert(property)
                        ? new ObjectPropertyConverter(property, maps)
                        : null;
                }

                public NameValueCollection Convert(object instance, ISet<object> processed)
                {
                    if (instance != null)
                    {
                        var value = _property.GetValue(instance);
                        if (value != null)
                        {
                            var converter = TryGetNonObjectConverter(value);
                            if (converter != null)
                                return converter.Convert(value);

                            return ConvertObject(value, processed);
                        }
                    }

                    return new NameValueCollection();
                }

                IValueConverter TryGetNonObjectConverter(object value)
                {
                    var type = value.GetType();

                    if (NameValueCollectionValueConverter.CanConvert(type))
                        return new NameValueCollectionValueConverter(_name);

                    if (SimpleDictionaryValueConverter.CanConvert(type))
                        return new SimpleDictionaryValueConverter(_name, _format);

                    if (SimpleCollectionValueConverter.CanConvert(type))
                        return new SimpleCollectionValueConverter(GetNonEmptyPropertyName(), _format);

                    return SimpleValueConverter.CanConvert(type)
                        ? new SimpleValueConverter(GetNonEmptyPropertyName(), _format)
                        : null;
                }

                string GetNonEmptyPropertyName()
                {
                    return string.IsNullOrWhiteSpace(_name) ? _property.Name : _name;
                }

                NameValueCollection ConvertObject(object value, ISet<object> processed)
                {
                    if (processed.Contains(value))
                        return new NameValueCollection();

                    processed.Add(value);

                    var nestedValues = _maps.Convert(value, processed);

                    var values = new NameValueCollection();

                    var baseName = GetNonEmptyPropertyName();

                    foreach (var key in nestedValues.AllKeys)
                    {
                        var vvs = nestedValues.GetValues(key);
                        if (vvs != null)
                        {
                            foreach (var vv in vvs)
                            {
                                values.Add(baseName + "." + key, vv);
                            }
                        }
                    }

                    return values;
                }

                static bool CanConvert(PropertyInfo property)
                {
                    return !property.IsIndexer() && property.GetGetMethod() != null;
                }
            }
        }
    }
}