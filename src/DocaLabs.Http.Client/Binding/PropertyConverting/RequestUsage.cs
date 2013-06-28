using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines where a property should be mapped, e.g. URL's query, path or request headers.
    /// 
    /// </summary>
    [Flags]
    public enum RequestUsage
    {
        /// <summary>
        /// Ignore.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// To the URL's query.
        /// </summary>
        InQuery = 1,

        /// <summary>
        /// To the URL's path.
        /// </summary>
        InPath = 2,

        /// <summary>
        /// To the web request header.
        /// </summary>
        InHeader = 4,

        /// <summary>
        /// To the web request's form.
        /// </summary>
        InRequestForm = 8
    }
}
