namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines values that influence which culture should be used for converting values using specified formats.
    /// </summary>
    public enum FormatCulture
    {
        /// <summary>
        /// Use CultureInfo.InvariantCulture
        /// </summary>
        UseInvariant = 0,

        /// <summary>
        /// Use CultureInfo.CurrentCulture
        /// </summary>
        UseCurrent = 1,

        /// <summary>
        /// CultureInfo.CurrentUICulture
        /// </summary>
        UseCurrentUI = 2
    }
}