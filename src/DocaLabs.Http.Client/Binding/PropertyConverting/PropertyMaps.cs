﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
        /// Converts instance into NameValueCollection where keys/values correspond to property names/values.
        /// </summary>
        /// <param name="instance">Value to be converted.</param>
        /// <param name="acceptPropertyCheck">Delegate which is used to check whenever the passed property should be parsed.</param>
        public NameValueCollection Convert(object instance, Func<PropertyInfo, bool> acceptPropertyCheck)
        {
            if (acceptPropertyCheck == null)
                throw new ArgumentNullException("acceptPropertyCheck");

            return Convert(instance, new HashSet<object>(), acceptPropertyCheck);
        }

        /// <summary>
        /// Return IValueConverter if the model can be converted by NameValueCollectionValueConverter or SimpleDictionaryValueConverter.
        /// </summary>
        public static IValueConverter TryGetDictionaryModelValueConverter(object model)
        {
            if (model == null)
                return null;

            var type = model.GetType();

            if (NameValueCollectionValueConverter.CanConvert(type))
                return new NameValueCollectionValueConverter(null);

            return SimpleDictionaryValueConverter.CanConvert(type)
                ? new SimpleDictionaryValueConverter(null, null, null)
                : null;
        }

        /// <summary>
        /// Return true the model can be converted by NameValueCollectionValueConverter or SimpleDictionaryValueConverter.
        /// </summary>
        public static bool IsDictionaryModel(Type type)
        {
            return type != null && (NameValueCollectionValueConverter.CanConvert(type) || SimpleDictionaryValueConverter.CanConvert(type));
        }

        internal NameValueCollection Convert(object instance, ISet<object> processed, Func<PropertyInfo, bool> acceptPropertyCheck)
        {
            if (instance == null)
                return new NameValueCollection();

            var type = instance.GetType();

            PropertyMap map;

            if (_maps.TryGetValue(type, out map))
                return map.Convert(instance, processed);

            map = new PropertyMap(this);

            _maps.TryAdd(type, map);

            map.Parse(instance, acceptPropertyCheck);

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

            internal void Parse(object instance, Func<PropertyInfo, bool> acceptPropertyCheck)
            {
                var type = instance.GetType();

                _converters = SkipParsing(type)
                    ? new List<IPropertyConverter>()
                    : type.GetAllPublicInstanceProperties()
                            .Select(x => ParseProperty(x, acceptPropertyCheck))
                            .Where(x => x != null)
                            .ToList();
            }

            internal NameValueCollection Convert(object instance, ISet<object> processed)
            {
                var modelConverter = TryGetDictionaryModelValueConverter(instance);
                if (modelConverter != null)
                    return modelConverter.Convert(instance);

                var values = new NameValueCollection();

                if (_converters == null || _converters.Count == 0)
                    return values;

                processed.Add(instance);

                foreach (var converter in _converters)
                    values.Add(converter.Convert(instance, processed));

                return values;
            }

            IPropertyConverter ParseProperty(PropertyInfo propertyInfo, Func<PropertyInfo, bool> acceptPropertyCheck)
            {
                return acceptPropertyCheck(propertyInfo)
                    ? GetConverter(propertyInfo, acceptPropertyCheck)
                    : null;
            }

            IPropertyConverter GetConverter(PropertyInfo property, Func<PropertyInfo, bool> acceptPropertyCheck)
            {
                return TryGetCustomPropertyParser(property)
                    ?? NameValueCollectionPropertyConverter.TryCreate(property)
                    ?? SimpleDictionaryPropertyConverter.TryCreate(property)
                    ?? SimpleCollectionPropertyConverter.TryCreate(property)
                    ?? SimplePropertyConverter.TryCreate(property)
                    ?? ObjectPropertyConverter.TryCreate(property, _maps, acceptPropertyCheck);
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
                return type == typeof(object) || type.IsSimpleType() || IsDictionaryModel(type);
            }

            class ObjectPropertyConverter : IPropertyConverter
            {
                readonly PropertyMaps _maps;
                readonly PropertyInfo _property;
                readonly string _name;
                readonly string _format;
                readonly CultureInfo _culture;
                readonly Func<PropertyInfo, bool> _acceptPropertyCheck;

                ObjectPropertyConverter(PropertyInfo property, PropertyMaps maps, Func<PropertyInfo, bool> acceptPropertyCheck)
                {
                    _property = property;
                    _maps = maps;
                    _acceptPropertyCheck = acceptPropertyCheck;

                    var requestUse = property.GetCustomAttribute<PropertyOverridesAttribute>();
                    if (requestUse != null)
                    {
                        _name = requestUse.Name;
                        _format = requestUse.Format;
                        _culture = requestUse.GetFormatCultureInfo();
                    }

                    if (_name == null)
                        _name = _property.Name;
                }

                public static IPropertyConverter TryCreate(PropertyInfo property, PropertyMaps maps, Func<PropertyInfo, bool> acceptPropertyCheck)
                {
                    return CanConvert(property)
                        ? new ObjectPropertyConverter(property, maps, acceptPropertyCheck)
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

                            if(string.IsNullOrWhiteSpace(_format))
                                return ConvertObject(value, processed);

                            return new NameValueCollection
                            {
                                { GetNonEmptyPropertyName(), string.Format(_format, value) }
                            };
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
                        return new SimpleDictionaryValueConverter(_name, _format, _culture);

                    if (SimpleCollectionValueConverter.CanConvert(type))
                        return new SimpleCollectionValueConverter(GetNonEmptyPropertyName(), _format, _culture);

                    return SimpleValueConverter.CanConvert(type)
                        ? new SimpleValueConverter(GetNonEmptyPropertyName(), _format, _culture)
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

                    var nestedValues = _maps.Convert(value, processed, _acceptPropertyCheck);

                    var values = new NameValueCollection();

                    var baseName = _name ?? _property.Name;

                    foreach (var key in nestedValues.AllKeys)
                    {
                        var vvs = nestedValues.GetValues(key);
                        if (vvs != null)
                        {
                            foreach (var vv in vvs)
                            {
                                values.Add(string.IsNullOrWhiteSpace(baseName) ? key : baseName + "." + key, vv);
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