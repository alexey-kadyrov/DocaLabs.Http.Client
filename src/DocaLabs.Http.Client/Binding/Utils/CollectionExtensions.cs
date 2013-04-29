using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.Utils
{
    public static class CollectionExtensions
    {
        public static void Add(this NameValueCollection collection, string key, IEnumerable<string> values)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");

            if(values == null)
                return;

            foreach (var value in values)
                collection.Add(key, value);
        }
    }
}
