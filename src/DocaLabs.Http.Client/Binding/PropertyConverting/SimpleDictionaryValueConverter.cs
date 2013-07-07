using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts IDictionary type values.
    /// </summary>
    public class SimpleDictionaryValueConverter : IValueConverter
    {
        readonly string _name;
        readonly string _format;

        /// <summary>
        /// Initializes an instance of the SimpleDictionaryValueConverter class.
        /// </summary>
        /// <param name="name">
        /// If the Name is not empty then it will be added to the key from the collection, e.g. key = Name + "." + itemKey. 
        /// Otherwise the key from the collection is used.
        /// </param>
        /// <param name="format">If the format is non empty then string.Format is used for converting values of the dictionary.</param>
        public SimpleDictionaryValueConverter(string name, string format)
        {
            _name = name;
            _format = format;
        }

        /// <summary>
        /// Converts a value.
        /// If the instance is null or the value of the property is null then the return collection will be empty.
        /// If the Name is not empty then it will be added to the key from the collection, e.g. key = Name + "." + itemKey.
        /// Otherwise the key from the collection is used.
        /// </summary>
        /// <param name="value">The IDictionary.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            var collection = GetWrapper(value);

            if (collection != null)
            {
                var makeName = GetNameMaker();

                foreach (var objKey in collection.Keys)
                {
                    if(objKey == null || !objKey.GetType().IsSimpleType())
                        continue;

                    var key = CustomConverter.Current.ChangeType<string>(objKey);

                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    var destKey = makeName(key);

                    if (string.IsNullOrWhiteSpace(destKey))
                        continue;

                    var v = collection[objKey];

                    if( v != null && !v.GetType().IsSimpleType())
                        continue;

                    if (v != null)
                        values.Add(destKey, CustomConverter.ChangeToString(_format, v) ?? "");
                    else
                        values.Add(destKey, "");
                }
            }

            return values;
        }

        /// <summary>
        /// Returns whenever the type can be converted by the SimpleDictionaryValueConverter.
        /// </summary>
        /// <returns>True if the type is or implements IDictionary or IDictionary{,}.</returns>
        public static bool CanConvert(Type type)
        {
            return typeof (IDictionary).IsAssignableFrom(type) || GetGenericDictionaryInterfaceDefinition(type) != null;
        }

        static Type GetGenericDictionaryInterfaceDefinition(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return type;

            return type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>));
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

        static IDictionaryWrapper GetWrapper(object instance)
        {
            if (instance == null)
                return null;

            var dic = instance as IDictionary;
            if(dic != null)
                return new DictionaryWrapper(dic);

            var dictionaryInterfaceDefinition = GetGenericDictionaryInterfaceDefinition(instance.GetType());
            return dictionaryInterfaceDefinition != null
                ? new GenericDictionaryWrapper(instance, dictionaryInterfaceDefinition) 
                : null;
        }

        interface IDictionaryWrapper
        {
            IEnumerable Keys { get; }
            object this[object key] { get; }
        }

        class DictionaryWrapper : IDictionaryWrapper
        {
            readonly IDictionary _dictionary;

            public DictionaryWrapper(IDictionary dictionary)
            {
                _dictionary = dictionary;
            }

            public IEnumerable Keys { get { return _dictionary.Keys; } }

            public object this[object key]
            {
                get { return _dictionary[key]; }
            }
        }

        class GenericDictionaryWrapper : IDictionaryWrapper
        {
            readonly PropertyInfo _keysProperty;
            readonly PropertyInfo _thisProperty;
            readonly object _dictionary;

            public GenericDictionaryWrapper(object dictionary, Type dictionaryInterfaceDefinition)
            {
                _dictionary = dictionary;
                _keysProperty = dictionaryInterfaceDefinition.GetProperty("Keys");
                _thisProperty = dictionaryInterfaceDefinition.GetProperty("Item");
            }

            public IEnumerable Keys { get { return _keysProperty.GetValue(_dictionary) as IEnumerable; } }

            public object this[object key]
            {
                get { return _thisProperty.GetValue(_dictionary, new [] { key }); }
            }
        }
    }
}