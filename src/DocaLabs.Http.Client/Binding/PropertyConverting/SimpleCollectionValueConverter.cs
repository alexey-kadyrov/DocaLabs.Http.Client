using System.Collections;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public class SimpleCollectionValueConverter : IValueConverter
    {
        readonly string _name;
        readonly string _format;

        public SimpleCollectionValueConverter(string name, string format)
        {
            _name = name;
            _format = format;
        }

        /// <summary>
        /// Converts a collection value. If the value is null then the return collection will be empty.
        /// </summary>
        /// <param name="value">The collection.</param>
        /// <returns>One key-values pair.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            var collection = value as IEnumerable;

            if (collection != null)
            {
                foreach (var item in collection)
                    values.Add(_name, CustomConverter.ChangeToString(_format, item));
            }

            return values;
        }
    }
}
