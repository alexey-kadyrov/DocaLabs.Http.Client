using System;
using System.Collections.Specialized;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts NameValueCollection type properties.
    /// </summary>
    public class NameValueCollectionPropertyConverter : PropertyConverterBase, IConverter 
    {
        NameValueCollectionPropertyConverter(PropertyInfo property)
            : base(property)
        {
            if (Name == null)
                Name = property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is derived from NameValueCollection
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(Type type, PropertyInfo property)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if (property == null)
                throw new ArgumentNullException("property");

            return CanConvert(type)
                ? new NameValueCollectionPropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the obj is null or the value of the property is null then the return collection will be empty.
        /// If the Name was overridden (The IsOverridden is true) then it will be added to the key from the collection,
        /// e.g. key = Name + "." + itemKey
        /// </summary>
        /// <param name="value">Value of the property.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            if (value != null)
                TryAddValues(value, values);

            return values;
        }

        void TryAddValues(object value, NameValueCollection values)
        {
            var collection = value as NameValueCollection;
            if (collection == null)
                return;

            var makeName = GetNameMaker();

            foreach (var key in collection.AllKeys)
                values.Add(makeName(key), collection[key]);
        }

        static bool CanConvert(Type type)
        {
            return typeof(NameValueCollection).IsAssignableFrom(type);
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