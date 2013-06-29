﻿using System;
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
                Name = Property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is derived from NameValueCollection
        ///     * Is not an indexer
        /// </summary>
        public static IConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new NameValueCollectionPropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the obj is null or the value of the property is null then the return collection will be empty.
        /// If the Name was overridden (The IsOverridden is true) then it will be added to the key from the collection,
        /// e.g. key = Name + "." + itemKey
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
            var collection = Property.GetValue(obj, null) as NameValueCollection;
            if (collection == null)
                return;

            var makeName = GetNameMaker();

            foreach (var key in collection.AllKeys)
                values.Add(makeName(key), collection[key]);
        }

        static bool CanConvert(PropertyInfo property)
        {
            return typeof(NameValueCollection).IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0;
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