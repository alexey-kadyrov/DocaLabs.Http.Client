using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    public class ContentType
    {
        public const string Default = "application/octet-stream";

        readonly string _mediaType;
        readonly string _subType;
        readonly Dictionary<string, string> _parameters;

        public string OriginalValue { get; private set; }

        public string CharSet
        {
            get
            {
                string value;
                _parameters.TryGetValue("charset", out value);
                return !string.IsNullOrWhiteSpace(value)
                    ? value
                    : _mediaType == "text"
                        ? "ISO-8859-1"
                        : string.Empty;
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary> 
        public string MediaType
        {
            get
            {
                return _mediaType + "/" + _subType;
            }
        }

        public ContentType(string originalValue, string mediaType, string subType, Dictionary<string, string> parameters)
        {
            if(string.IsNullOrWhiteSpace(originalValue))
                throw new ArgumentNullException("originalValue");

            if(string.IsNullOrWhiteSpace(mediaType))
                throw new ArgumentNullException("mediaType");

            if(string.IsNullOrWhiteSpace(_subType))
                throw  new ArgumentNullException("subType");

            if(parameters == null)
                throw  new ArgumentNullException("parameters");

            OriginalValue = originalValue;
            _mediaType = mediaType;
            _subType = subType;
            _parameters = parameters;
        }

        public string GetParameter(string name)
        {
            string value;
            _parameters.TryGetValue("charset", out value);
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value;
        }
    }
}
