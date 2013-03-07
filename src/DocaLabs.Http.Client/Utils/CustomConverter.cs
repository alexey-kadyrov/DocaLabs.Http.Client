using System;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Converts a data type to another base data type.
    /// </summary>
    public class CustomConverter
    {
        /// <summary>
        /// Current converter.
        /// </summary>
        public static CustomConverter Current { get { return DefaultLazyCustomConverter.LazyConverter; } }

        /// <summary>
        /// Gets current custom converter factory.
        /// </summary>
        public ICustomConverterFactory Factory { get; private set; }

        /// <summary>
        /// Initializes an instance of CustomConverter class with default converter factory
        /// </summary>
        public CustomConverter()
            : this(new CustomConverterFactory())
        {
        }

        /// <summary>
        /// Initializes an instance of CustomConverter class with the specified converter factory
        /// </summary>
        public CustomConverter(ICustomConverterFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;
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

        /// <summary>
        /// Specifies the default value of the type parameter.
        /// </summary>
        /// <typeparam name="T">A reference type or a value type.</typeparam>
        /// <returns>Default value for the type.</returns>
        public static T GetDefault<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Specifies the default value of the type parameter.
        /// </summary>
        /// <param name="type">A reference type or a value type.</param>
        /// <returns>Default value for the type.</returns>
        public static object GetDefaultValue(Type type)
        {
            return typeof(CustomConverter).GetMethod("GetDefault").MakeGenericMethod(type).Invoke(null, null);
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
