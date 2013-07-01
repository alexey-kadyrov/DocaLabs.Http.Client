using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
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

        public NameValueCollection Convert(object instance)
        {
            var values = new NameValueCollection();

            if (_converters == null)
                return values;

            foreach (var converter in _converters)
                values.Add(converter.Convert(instance));

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
                ?? SimplePropertyConverter.TryCreate(property)
                ?? SimpleCollectionPropertyConverter.TryCreate(property)
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

            public NameValueCollection Convert(object instance)
            {
                if (instance != null)
                {
                    var value = _property.GetValue(instance);
                    if (value != null)
                    {
                        var converter = GetConverter(value);
                        if (converter != null)
                            return converter.Convert(value);

                        return ConvertObject(value);
                    }
                }

                return new NameValueCollection();
            }

            IValueConverter GetConverter(object value)
            {
                if (value is NameValueCollection)
                    return new NameValueCollectionValueConverter(_name, _format);

                var type = value.GetType();

                if (type.IsSimpleType())
                    return new SimpleValueConverter(_name, _format);

                if (type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType())
                    return new SimpleCollectionValueConverter(_name, _format);

                return null;
            }

            NameValueCollection ConvertObject(object value)
            {
                var makeName = GetNameMaker();

                var nestedValues = _maps.GetOrAdd(value).Convert(value);

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
                return !property.IsIndexer();
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