using System;
using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts NameValueCollection type properties.
    /// </summary>
    public class NameValueCollectionValueConverter : IValueConverter 
    {
        readonly string _name;
        readonly string _format;

        public NameValueCollectionValueConverter(string name, string format)
        {
            _name = name;
            _format = format;
        }

        /// <summary>
        /// Converts a value of NameValueCollection type.
        /// If the value is null then the return collection will be empty.
        /// If the Name was overridden (The IsOverridden is true) then it will be added to the key from the collection,
        /// e.g. key = Name + "." + itemKey
        /// </summary>
        /// <param name="value">The NameValueCollection.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            var collection = value as NameValueCollection;

            if (collection != null)
            {
                var makeName = GetNameMaker();

                foreach (var key in collection.AllKeys)
                    values.Add(makeName(key), collection[key]);
            }

            return values;
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