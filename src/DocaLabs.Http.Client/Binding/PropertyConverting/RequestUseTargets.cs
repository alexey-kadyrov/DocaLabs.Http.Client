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
        /// Ignore.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// To the URL's query.
        /// </summary>
        UrlQuery = 1,

        /// <summary>
        /// To the URL's path.
        /// </summary>
        UrlPath = 2,

        /// <summary>
        /// To the web request header.
        /// </summary>
        RequestHeader = 4,

        /// <summary>
        /// To the web request's form.
        /// </summary>
        RequestBodyAsForm = 8
    }
}
