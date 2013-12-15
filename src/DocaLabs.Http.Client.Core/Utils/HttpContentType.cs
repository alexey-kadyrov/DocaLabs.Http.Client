using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    public class HttpContentType
    {
        public const string Default = "application/octet-stream";

        string _type;
        string _subType;
        Dictionary<string, string> _parameters;

        public string OriginalValue { get; private set; }

        public string CharSet
        {
            get
            {
                string value;
                _parameters.TryGetValue("charset", out value);
                return string.IsNullOrWhiteSpace(value)
                    ? _type.ToLowerInvariant() == "text" ? "ISO-8859-1" : string.Empty
                    : value;
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary> 
        public string MediaType
        {
            get
            {
                return _type + "/" + _subType;
            }
        }

        public HttpContentType(string originalValue)
        {
            OriginalValue = string.IsNullOrWhiteSpace(originalValue) 
                ? Default
                : originalValue;

            Parse();
        }

        public string GetParameter(string attribute)
        {
            string value;
            _parameters.TryGetValue("charset", out value);
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value;
        }

        void Parse()
        {
            var parser = new HeaderParser(OriginalValue);

            _type = parser.ReadToken();
            if (string.IsNullOrWhiteSpace(_type) || !parser.Is('/'))
                throw new FormatException(string.Format(Resources.Text.malformed_content_type, OriginalValue));

            _subType = parser.ReadToken();
            if (string.IsNullOrWhiteSpace(_subType))
                throw new FormatException(string.Format(Resources.Text.malformed_content_type, OriginalValue));

            _parameters = parser.ReadParameters();
        }
    }
}
