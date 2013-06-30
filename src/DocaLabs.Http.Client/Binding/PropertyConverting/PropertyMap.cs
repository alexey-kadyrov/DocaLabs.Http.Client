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
        readonly List<IConverter> _converters;

        public PropertyMap(PropertyMaps maps)
        {
            if(maps == null)
                throw new ArgumentNullException("maps");

            _maps = maps;
            _converters = new List<IConverter>();
        }

        public void Parse(object o)
        {
            if(o == null)
                throw new ArgumentNullException("o");

            var type = o.GetType();

            _converters.Clear();

            if (!type.IsSimpleType())
            {
                _converters.AddRange(
                    type.GetAllPublicInstanceProperties()
                        .Select(x => ParseProperty(x, o))
                        .Where(x => x != null)
                        .ToList());
            }
        }

        public NameValueCollection Convert(object o)
        {
            var values = new NameValueCollection();

            foreach (var converter in _converters)
                values.Add(converter.Convert(o));

            return values;
        }

        IConverter ParseProperty(PropertyInfo property, object o)
        {
            return _maps.AcceptPropertyCheck(property)
                ? GetConverter(property, o) 
                : null;
        }

        IConverter GetConverter(PropertyInfo property, object o)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property)
                ?? NameValueCollectionPropertyConverter.TryCreate(property)
                ?? SimpleCollectionPropertyConverter.TryCreate(property)
                ?? ObjectPropertyConverter.TryCreate(property, o, _maps);
        }

        static IConverter TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }

        class ObjectPropertyConverter : PropertyConverterBase, IConverter
        {
            readonly PropertyMaps _maps;

            ObjectPropertyConverter(PropertyInfo property, PropertyMaps maps)
                : base(property)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    Name = Property.Name;

                _maps = maps;
            }

            public static IConverter TryCreate(PropertyInfo property, object o, PropertyMaps maps)
            {
                if (property == null)
                    throw new ArgumentNullException("property");

                if (!CanConvert(property))
                    return null;

                return new ObjectPropertyConverter(property, maps);
            }

            public NameValueCollection Convert(object obj)
            {
                var values = new NameValueCollection();

                if (obj != null)
                    TryAddValues(obj, values);

                return values;
            }

            void TryAddValues(object o, NameValueCollection values)
            {
                var value = Property.GetValue(o, null);

                var makeName = GetNameMaker();

                if (value == null)
                    return;

                var nestedValues = _maps.GetOrAdd(o).Convert(value);

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

            static bool CanConvert(PropertyInfo property)
            {
                var type = property.PropertyType;

                return !type.IsSimpleType() && !type.IsEnumerable() && property.GetIndexParameters().Length == 0;
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