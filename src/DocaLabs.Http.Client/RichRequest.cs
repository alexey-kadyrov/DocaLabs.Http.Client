using System;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Defines additional information for the web request.
    /// </summary>
    public class RichRequest
    {
        /// <summary>
        /// Defines DateTime format string for RFC 1123.
        /// </summary>
        public const string Rfc1123DateFormat = "{0:ddd, dd MMM yyyy HH:mm:ss} GMT";

        /// <summary>
        /// Gets or sets the value of 'If-Match' header.
        /// The header will be set if the value is non empty string.
        /// </summary>
        [RequestUse(RequestUseTargets.RequestHeader, "If-Match")]
        public string IfMatch { get; set; }

        /// <summary>
        /// Gets or sets the value of the 'If-None-Match' header.
        /// The header will be set if the value is non empty string.
        /// </summary>
        [RequestUse(RequestUseTargets.RequestHeader, "If-None-Match")]
        public string IfNoneMatch { get; set; }

        /// <summary>
        /// Gets or sets the value of the 'If-Modified-Since' header.
        /// The header will be set if the value is not null.
        /// If set the value is assumed to be UTC.
        /// </summary>
        [RequestUse(RequestUseTargets.RequestHeader, "If-Modified-Since", Format = Rfc1123DateFormat)]
        public DateTime? IfModifiedSince { get; set; }

        /// <summary>
        /// Gets or sets the value of the 'If-Unmodified-Since' header.
        /// The header will be set if the value is not null.
        /// If set the value is assumed to be UTC.
        /// </summary>
        [RequestUse(RequestUseTargets.RequestHeader, "If-Unmodified-Since", Format = Rfc1123DateFormat)]
        public DateTime? IfUnmodifiedSince { get; set; }
    }


    /// <summary>
    /// A generic that can be used to wrap the input model in order to add more information for the request, such as 'If-Match', 'If-Modified-Since' headers.
    /// </summary>
    /// <typeparam name="T">Your input model.</typeparam>
    public class RichRequest<T> : RichRequest
    {
        /// <summary>
        /// Gets the value of the input model.
        /// </summary>
        [PropertyOverrides(Name = "")]
        public T Value { get; set; }

        /// <summary>
        /// Initializes an instance of the RichRequest class with the default value.
        /// </summary>
        public RichRequest()
        {
        }

        /// <summary>
        /// Initializes an instance of the RichRequest class with the specified value.
        /// </summary>
        public RichRequest(T value)
        {
            Value = value;
        }
    }
}
