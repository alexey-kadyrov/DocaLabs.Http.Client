using System;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client
{
    public class RichRequest
    {
        public const string Rfc1123DateFormat = "{0:ddd, dd MMM yyyy HH:mm:ss} GMT";

        [RequestUse(RequestUseTargets.RequestHeader, "If-Match")]
        public string IfMatch { get; set; }

        [RequestUse(RequestUseTargets.RequestHeader, "If-Modified-Since", Format = Rfc1123DateFormat)]
        public DateTime? IfModifiedSince { get; set; }
    }

    public class RichRequest<T> : RichRequest
    {
        [PropertyOverrides(Name = "")]
        public T Value { get; set; }

        public RichRequest()
        {
        }

        public RichRequest(T value)
        {
            Value = value;
        }
    }
}
