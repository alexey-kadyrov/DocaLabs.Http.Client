using System;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts CustomNameValueCollection type properties.
    /// </summary>
    public class CustomNameValueCollectionPropertyConverter : PropertyConverterBase, IPropertyConverter 
    {
        CustomNameValueCollectionPropertyConverter(PropertyInfo property, IPropertyConverterOverrides overrides)
            : base(property, overrides)
        {
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is derived from CustomNameValueCollection
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return typeof (CustomNameValueCollection).IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0
                ? new CustomNameValueCollectionPropertyConverter(property, overrides) 
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
        public CustomNameValueCollection Convert(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
                TryAddValues(obj, values);

            return values;
        }

        void TryAddValues(object obj, IDictionaryList<string, string> values)
        {
            var collection = Property.GetValue(obj, null) as CustomNameValueCollection;
            if (collection == null)
                return;

            var makeKey = GetKeyMaker();

            foreach (var pair in collection)
                values.Add(makeKey(pair.Key), pair.Value);
        }

        Func<string, string> GetKeyMaker()
        {
            return IsNameOverridden
                ? (Func<string, string>) MakeCompositeName
                : GetNameAsIs;
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