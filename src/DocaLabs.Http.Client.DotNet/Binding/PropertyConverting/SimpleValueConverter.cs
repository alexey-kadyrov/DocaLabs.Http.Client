using System;
using System.Collections.Specialized;
using System.Globalization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts simple values, like int, string, Guid, etc.
    /// </summary>
    public class SimpleValueConverter : IValueConverter
    {
        readonly string _name;
        readonly string _format;
        readonly CultureInfo _culture;

        /// <summary>
        /// Initializes an instance of the SimpleValueConverter class.
        /// </summary>
        /// <param name="name">Name which should be used as the key for the converted value.</param>
        /// <param name="format">If the format is non empty then string.Format is used for converting.</param>
        /// <param name="culture">Culture to be used if the format is non empty string.</param>
        public SimpleValueConverter(string name, string format, CultureInfo culture)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _name = name;
            _format = format;
            _culture = culture;
        }

        /// <summary>
        /// Converts a value using specified name as the key in the return dictionary.
        /// If the value is null then the return collection will be empty.
        /// If the format passed into the constructor is not empty string then string.Format is used.
        /// </summary>
        /// <param name="value">Value which should be converted.</param>
        /// <returns>One key-value pair.</returns>
        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            if (value != null)
                values.Add(_name, CustomConverter.ChangeToString(_format, _culture, value));

            return values;
        }

        /// <summary>
        /// Returns whenever the type can be converted by the SimpleValueConverter.
        /// </summary>
        /// <returns>True if the type is simple, e.g. int, double, string, byte[], DateTime.</returns>
        public static bool CanConvert(Type type)
        {
            return type.IsSimpleType();
        }
    }
}
