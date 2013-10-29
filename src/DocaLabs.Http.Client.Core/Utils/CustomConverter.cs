using System;
using System.Globalization;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Converts a data type to another base data type.
    /// </summary>
    public class CustomConverter
    {
        static volatile CustomConverter _current;

        /// <summary>
        /// Gets or sets the current converter. The property never returns null, if the value is not set or is null 
        /// then the CustomConverter is used in the default configuration.
        /// </summary>
        public static CustomConverter Current
        {
            get { return _current ?? DefaultLazyCustomConverter.LazyConverter; }
            set { _current = value; }
        }

        /// <summary>
        /// Gets current custom converter factory.
        /// </summary>
        public ICustomConverterFactory Factory { get; private set; }

        /// <summary>
        /// Initializes an instance of CustomConverter class with default converter factory.
        /// </summary>
        public CustomConverter()
            : this(new CustomConverterFactory())
        {
        }

        /// <summary>
        /// Initializes an instance of CustomConverter class with the specified converter factory.
        /// </summary>
        public CustomConverter(ICustomConverterFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;
        }


        /// <summary>
        /// Returns a string equivalent of the specified object.
        /// If the value is null then empty string is returned. If the format is specified then string.Format is used.
        /// If culture is null then CultureInfo.InvariantCulture is used for string.Format.
        /// Otherwise it uses the custom converter to convert into the string.
        /// </summary>
        public static string ChangeToString(string format, CultureInfo culture, object value)
        {
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(format)
                ? Current.ChangeType<string>(value)
                : string.Format(culture ?? CultureInfo.InvariantCulture, format, value);
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="value">An object to be converted.</param>
        /// <returns>An object whose type is T and whose value is equivalent to value.</returns>
        public virtual T ChangeType<T>(object value)
        {
            return (T)ChangeType(value, typeof (T));
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object to be converted.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <returns>An object whose type is conversionType and whose value is equivalent to value.</returns>
        public virtual object ChangeType(object value, Type conversionType)
        {
            var converter = Factory.GetConverter(conversionType);

            return converter == null ? Convert.ChangeType(value, conversionType) : converter(value);
        }

        static class DefaultLazyCustomConverter
        {
            internal static CustomConverter LazyConverter { get; private set; }

            static DefaultLazyCustomConverter()
            {
                LazyConverter = new CustomConverter();
            }
        }
    }
}
