using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DocaLabs.Http.Client.Utils
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public static List<T> Distinct<T>(this IEnumerable<T> original, Func<T, T, bool> areEqual)
        {
            var distinct = new List<T>();

            foreach (var item in original)
            {
                if (distinct.All(added => !areEqual(item, added)))
                    distinct.Add(item);
            }

            return distinct;
        }
    }
}
