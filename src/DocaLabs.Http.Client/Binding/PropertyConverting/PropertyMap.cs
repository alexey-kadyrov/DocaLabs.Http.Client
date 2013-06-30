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
        List<MappedProperty> _properties;

        internal PropertyMap(PropertyMaps maps)
        {
            if(maps == null)
                throw new ArgumentNullException("maps");

            _maps = maps;
            _properties = new List<MappedProperty>();
        }

        internal void Parse(object o)
        {
            if(o == null)
                throw new ArgumentNullException("o");

            var type = o.GetType();

            _properties = type.IsSimpleType() 
                ? new List<MappedProperty>() 
                : type.GetAllPublicInstanceProperties()
                    .Select(x => ParseProperty(o, x))
                    .Where(x => x != null)
                    .ToList();
        }

        public NameValueCollection Convert(object o)
        {
            var values = new NameValueCollection();

            foreach (var property in _properties)
                values.Add(property.Converter.Convert(property.Info.GetValue(o, null)));

            return values;
        }

        MappedProperty ParseProperty(object o, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length > 0 || !_maps.AcceptPropertyCheck(property))
                return null;

            var value = property.GetValue(o, null);

            var converter = GetConverter(value != null ? value.GetType() : property.PropertyType, property);

            return converter == null
                ? null
                : new MappedProperty(property, converter);
        }

        IConverter GetConverter(Type type, PropertyInfo property)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(type, property)
                ?? NameValueCollectionPropertyConverter.TryCreate(type, property)
                ?? SimpleCollectionPropertyConverter.TryCreate(type, property)
                ?? ObjectPropertyConverter.TryCreate(type, property, _maps);
        }

        static IConverter TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }

        class MappedProperty
        {
            public PropertyInfo Info { get; private set; }
            public IConverter Converter { get; private set; }

            public MappedProperty(PropertyInfo info, IConverter converter)
            {
                Info = info;
                Converter = converter;
            }
        }

        class ObjectPropertyConverter : PropertyConverterBase, IConverter
        {
            readonly PropertyMaps _maps;

            ObjectPropertyConverter(PropertyInfo property, PropertyMaps maps)
                : base(property)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    Name = property.Name;

                _maps = maps;
            }

            public static IConverter TryCreate(Type type, PropertyInfo property, PropertyMaps maps)
            {
                if (property == null)
                    throw new ArgumentNullException("property");

                if (!CanConvert(type))
                    return null;

                return new ObjectPropertyConverter(property, maps);
            }

            public NameValueCollection Convert(object value)
            {
                var values = new NameValueCollection();

                if (value != null)
                    TryAddValues(value, values);

                return values;
            }

            void TryAddValues(object value, NameValueCollection values)
            {
                var makeName = GetNameMaker();

                if (value == null)
                    return;

                var nestedValues = _maps.GetOrAdd(value).Convert(value);

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
            }

            static bool CanConvert(Type type)
            {
                return !type.IsSimpleType() && !type.IsEnumerable();
            }

            Func<string, string> GetNameMaker()
            {
                return string.IsNullOrWhiteSpace(Name)
                    ? GetNameAsIs
                    : (Func<string, string>)MakeCompositeName;
            }

            static string GetNameAsIs(string name)
            {
                return name;
            }

            string MakeCompositeName(string name)
            {
                return Name + "." + name;
            }
        }
    }
}