using System;
using System.Collections.Generic;
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
        /// Adds a new pair key/value to the query string. The key and value are encoded using Uri.EscapeDataString.
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

            key = Uri.EscapeDataString(key);
            value = Uri.EscapeDataString(value);

            if (_builder.Length == 0)
                _builder.Append(key).Append("=").Append(value);
            else
                _builder.Append("&").Append(key).Append("=").Append(value);

            return this;
        }

        /// <summary>
        /// Adds a new pairs of key/value from collection to the query string. The key and value are encoded using Uri.EscapeDataString.
        /// </summary>
        /// <returns>Self reference.</returns>
        public QueryStringBuilder Add(ICustomKeyValueCollection collection)
        {
            if (collection == null)
                return this;

            foreach (var key in collection.AllKeys)
                CopyValues(key, collection.GetValues(key));

            return this;
        }

        /// <summary>
        /// Returns the query string.
        /// </summary>
        public override string ToString()
        {
            return _builder.ToString();
        }

        void CopyValues(string key, IEnumerable<string> values)
        {
            if (values == null)
                return;

            foreach (var value in values)
                Add(key, value);
        }
    }
}
