using System;

namespace DocaLabs.Conversion
{
    /// <summary>
    /// Defines methods to manage collection of converters.
    /// </summary>
    public interface ICustomConverterFactory
    {
        /// <summary>
        /// Registers a converter.
        /// </summary>
        /// <param name="type">A type which converter can change type to.</param>
        /// <param name="converter">The converter.</param>
        void RegisterConverter(Type type, Func<object, object> converter);

        /// <summary>
        /// Returns a registered converter.
        /// </summary>
        /// <param name="type">The lookup type.</param>
        /// <returns>If there is no a converter for the type it will be null; otherwise the converter.</returns>
        Func<object, object> GetConverter(Type type);
    }
}
