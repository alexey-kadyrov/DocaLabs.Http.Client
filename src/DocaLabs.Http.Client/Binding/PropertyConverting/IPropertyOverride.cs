namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Specifies additional information about a property for serializing into a web request.
    /// </summary>
    public interface IPropertyOverride
    {
        /// <summary>
        /// Gets or sets a name that overrides the property name which is used by default.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the custom format string that is used by a property converter.
        /// If the format is non white space string then the converter will use string.Format
        /// to convert the property value. If set the format must include curly brackets.
        /// </summary>
        string Format { get; }
    }
}
