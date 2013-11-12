using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Collection extension methods.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds values with the specified key to the ICustomKeyValueCollection.
        /// </summary>
        /// <param name="collection">The target collection where value should be added.</param>
        /// <param name="key">The key value to be used for all values.</param>
        /// <param name="values">Collection of values to be added.</param>
        public static void Add(this ICustomKeyValueCollection collection, string key, IEnumerable<string> values)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");

            if(values == null)
                return;

            foreach (var value in values)
                collection.Add(key, value);
        }

        public static void Add(this WebHeaderCollection collection, ICustomKeyValueCollection values)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (values == null)
                return;

            foreach (var key in values)
            {
                var headerValue = new StringBuilder();

                foreach (var value in values.GetValues(key))
                {
                    if (headerValue.Length > 0)
                        headerValue.Append(", ");

                    headerValue.Append(value);
                }

                collection[key] = headerValue.ToString();
            }
        }
    }
}
