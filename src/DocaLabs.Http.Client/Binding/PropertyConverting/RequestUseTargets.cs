using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines where a property should be mapped, e.g. URL's query, path or request headers.
    /// 
    /// </summary>
    [Flags]
    public enum RequestUseTargets
    {
        /// <summary>
        /// Default uninitialized value.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Ignore.
        /// </summary>
        Ignore = 1,

        /// <summary>
        /// To the URL's query.
        /// </summary>
        UrlQuery = 2,

        /// <summary>
        /// To the URL's path.
        /// </summary>
        UrlPath = 4,

        /// <summary>
        /// To the web request header.
        /// </summary>
        RequestHeader = 8
    }
}
