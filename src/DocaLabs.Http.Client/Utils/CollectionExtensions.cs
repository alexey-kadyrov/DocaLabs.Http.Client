using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of value to ICollection{T}
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");

            if(items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                collection.Add(item);
        }
    }
}
