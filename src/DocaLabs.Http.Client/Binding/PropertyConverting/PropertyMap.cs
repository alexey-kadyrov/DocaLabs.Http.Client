using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Parses a type and maps its properties to name value pairs.
    /// </summary>
    public class PropertyMap
    {
        readonly PropertyMaps _maps;
        List<IPropertyConverter> _converters;

        internal PropertyMap(PropertyMaps maps)
        {
            if(maps == null)
                throw new ArgumentNullException("maps");

            _maps = maps;
        }

        internal void Parse(object instance)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            var type = instance.GetType();

            _converters = type.IsSimpleType()
                ? new List<IPropertyConverter>() 
                : type.GetAllPublicInstanceProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        /// <summary>
        /// Maps the instance to name value pairs using default rules or provided hints.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public NameValueCollection Convert(object instance)
        {
            if(instance == null)
                return new NameValueCollection();

            var type = instance.GetType();

            if (NameValueCollectionValueConverter.CanConvert(type))
                return new NameValueCollectionValueConverter(null).Convert(instance);

            return SimpleDictionaryValueConverter.CanConvert(type)
                ? new SimpleDictionaryValueConverter(null, null).Convert(instance)
                : Convert(instance, new HashSet<object>());
        }

        internal NameValueCollection Convert(object instance, ISet<object> processed)
        {
            var values = new NameValueCollection();

            if (instance == null || _converters == null || _converters.Count == 0)
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

                var requestUse = property.GetCustomAttribute<RequestUseAttribute>();
                if (requestUse != null)
                {
                    _name = requestUse.Name;
                    _format = requestUse.Format;
                }

                if (string.IsNullOrWhiteSpace(_name))
                    _name = _property.Name;
            }

            public static IPropertyConverter TryCreate(PropertyInfo property, PropertyMaps maps)
            {
                if (property == null)
                    throw new ArgumentNullException("property");

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
                if (value is NameValueCollection)
                    return new NameValueCollectionValueConverter(_name);

                var type = value.GetType();

                if (type.IsSimpleType())
                    return new SimpleValueConverter(_name, _format);

                if (type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType())
                    return new SimpleCollectionValueConverter(_name, _format);

                return null;
            }

            NameValueCollection ConvertObject(object value, ISet<object> processed)
            {
                if(processed.Contains(value))
                    return new NameValueCollection();

                processed.Add(value);

                var makeName = GetNameMaker();

                var nestedValues = _maps.Parse(value).Convert(value, processed);

                var values = new NameValueCollection();

                foreach (var key in nestedValues.AllKeys)
                {
                    var vvs = nestedValues.GetValues(key);
                    if (vvs != null)
                    {
                        foreach (var vv in vvs)
                        {
                            values.Add(makeName(key), vv);
                        }
                    }
                }

                return values;
            }

            static bool CanConvert(PropertyInfo property)
            {
                return !property.IsIndexer() && property.GetGetMethod() != null;
            }

            Func<string, string> GetNameMaker()
            {
                return string.IsNullOrWhiteSpace(_name)
                    ? GetNameAsIs
                    : (Func<string, string>)MakeCompositeName;
            }

            static string GetNameAsIs(string name)
            {
                return name;
            }

            string MakeCompositeName(string name)
            {
                return string.IsNullOrWhiteSpace(_name)
                    ? name
                    : _name + "." + name;
            }
        }
    }
}