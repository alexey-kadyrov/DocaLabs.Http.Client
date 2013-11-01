using System;
using System.Text;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Represents helper class to build a query string.
    /// </summary>
    public class QueryStringBuilder
    {
        readonly StringBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the QueryStringBuilder class.
        /// </summary>
        public QueryStringBuilder()
        {
            _builder = new StringBuilder();
        }

        /// <summary>
        /// Adds a new pair key/value to the query string. The key and value are encoded using HttpUtility.UrlEncode.
        /// </summary>
        /// <param name="key">Query parameter name.</param>
        /// <param name="value">Parameter's value.</param>
        /// <returns>Self reference.</returns>
        public QueryStringBuilder Add(string key, string value)
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if (value == null)
                return this;

            key = Uri.EscapeUriString(key);
            value = Uri.EscapeUriString(value);

            if (_builder.Length == 0)
                _builder.Append(key).Append("=").Append(value);
            else
                _builder.Append("&").Append(key).Append("=").Append(value);

            return this;
        }

        /// <summary>
        /// Adds a new pairs of key/value from collection to the query string. The key and value are encoded using HttpUtility.UrlEncode.
        /// </summary>
        /// <returns>Self reference.</returns>
        public QueryStringBuilder Add(NameValueCollection collection)
        {
            if (collection == null)
                return this;

            foreach (var key in collection.AllKeys)
            {
                var values = collection.GetValues(key);
                if (values != null)
                {
                    foreach (var value in values)
                        Add(key, value);
                }
            }

            return this;
        }

        /// <summary>
        /// Returns the query string.
        /// </summary>
        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
