using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts reference type properties, like object, etc.
    /// </summary>
    public class ObjectPropertyConverter : PropertyConverterBase, IConverter
    {
        readonly TypeConverter _typeConverter;

        ObjectPropertyConverter(PropertyInfo property, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, TypeConverter> processedTypes)
            : base(property)
        {
            if (string.IsNullOrWhiteSpace(Name))
                Name = Property.Name;

            _typeConverter = new TypeConverter(property.PropertyType, acceptPropertyCheck, processedTypes);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is not simple
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(PropertyInfo property, Func<PropertyInfo, bool> acceptPropertyCheck, ConcurrentDictionary<Type, TypeConverter> processedTypes)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            if (!CanConvert(property))
                return null;

            return processedTypes.ContainsKey(property.PropertyType) 
                ? null 
                : new ObjectPropertyConverter(property, acceptPropertyCheck, processedTypes);
        }

        /// <summary>
        /// Converts a property value.
        /// If the obj is null or the value of the property is null then the return collection will be empty.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object obj)
        {
            var values = new NameValueCollection();

            if (obj != null)
                TryAddValues(obj, values);

            return values;
        }

        void TryAddValues(object obj, NameValueCollection values)
        {
            var value = Property.GetValue(obj, null);

            var makeName = GetNameMaker();

            if (value == null)
                return;

            var nestedValues = _typeConverter.Convert(value);

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
