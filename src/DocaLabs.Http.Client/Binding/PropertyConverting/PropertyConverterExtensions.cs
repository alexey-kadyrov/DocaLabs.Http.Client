using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Extensions for property converters.
    /// </summary>
    public static class PropertyConverterExtensions
    {
        /// <summary>
        /// Tries to get suitable converter for a property, by trying in turns:
        ///     * Whenever descendant of the CustomPropertyConverterAttribute is defined on the property
        ///     * Can SimplePropertyConverter be used
        ///     * Can CustomNameValueCollectionPropertyConverter be used
        ///     * Can NameValueCollectionPropertyConverter be used
        ///     * Can CollectionPropertyConverter be used
        ///     * ObjectPropertyConverter be used
        /// The method may return null if none of the converters above can be used (one of the reason could be that the property is an indexer).
        /// </summary>
        public static IPropertyConverter GetConverter(this PropertyInfo property, IPropertyConverterOverrides overrides)
        {
            return TryGetCustomPropertyParser(property)
                ?? SimplePropertyConverter.TryCreate(property, overrides)
                ?? CustomNameValueCollectionPropertyConverter.TryCreate(property, overrides)
                ?? NameValueCollectionPropertyConverter.TryCreate(property, overrides)
                ?? CollectionPropertyConverter.TryCreate(property, overrides)
                ?? ObjectPropertyConverter.TryCreate(property, overrides);
        }

        static IPropertyConverter TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<CustomPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }
    }
}
