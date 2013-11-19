namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines empty type which can be used as a generic parameter for HttpClient in place of void.
    /// </summary>
    public struct VoidType
    {
        /// <summary>
        /// Default value.
        /// </summary>
        public static readonly VoidType Value = default (VoidType);
    }
}
