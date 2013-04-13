namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines overrides such as name and format for property converter.
    /// </summary>
    public interface IPropertyConverterOverrides
    {
        /// <summary>
        /// Gets the name which should be used.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the custom format string, if the format is non white space string then 
        /// the converter will use string.Format to convert the property value.
        /// The format must include curly brackets.
        /// </summary>
        string Format { get; }
    }
}
