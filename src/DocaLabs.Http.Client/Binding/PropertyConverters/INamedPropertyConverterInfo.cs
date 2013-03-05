namespace DocaLabs.Http.Client.Binding.PropertyConverters
{
    /// <summary>
    /// Defines properties for named for property converter info.
    /// </summary>
    public interface INamedPropertyConverterInfo
    {
        /// <summary>
        /// Gets the name which should be used. If RequestQueryAttribute is not defined or the Name in the attribute
        /// is null or blank then this property will set to the linked property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// gets the custom format string that is set by RequestQueryAttribute.
        /// </summary>
        string Format { get; }
    }
}
