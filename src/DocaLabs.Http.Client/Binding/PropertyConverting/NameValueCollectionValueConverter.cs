using System;
using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts NameValueCollection type values.
    /// </summary>
    public class NameValueCollectionValueConverter : IValueConverter 
    {
        readonly string _name;

        /// <summary>
        /// Initializes an instance of the NameValueCollectionValueConverter class.
        /// </summary>
        /// <param name="name">
        /// If the Name is not empty then it will be added to the key from the collection, e.g. key = Name + "." + itemKey. 
        /// Otherwise the key from the collection is used.
        /// </param>
        public NameValueCollectionValueConverter(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Converts a value.
        /// If the instance is null or the value of the property is null then the return collection will be empty.
        /// If the Name is not empty then it will be added to the key from the collection, e.g. key = Name + "." + itemKey.
        /// Otherwise the key from the collection is used.
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
                {
                    if(string.IsNullOrWhiteSpace(key))
                        continue;

                    var destKey = makeName(key);
                    if (string.IsNullOrWhiteSpace(destKey))
                        continue;

                    var vv = collection.GetValues(key);
                    if (vv != null)
                    {
                        foreach (var v in vv)
                            values.Add(destKey, v ?? "");
                    }
                    else
                    {
                        values.Add(destKey, "");
                    }
                }
            }

            return values;
        }

        /// <summary>
        /// Returns whenever the type can be converted by the NameValueCollectionValueConverter.
        /// </summary>
        /// <returns>True if the type is or derived from NameValueCollection.</returns>
        public static bool CanConvert(Type type)
        {
            return typeof(NameValueCollection).IsAssignableFrom(type);
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