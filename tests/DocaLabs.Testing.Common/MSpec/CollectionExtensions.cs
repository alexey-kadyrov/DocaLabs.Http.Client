using System.Collections.Generic;
using System.Collections.Specialized;
using Machine.Specifications;

namespace DocaLabs.Testing.Common.MSpec
{
    public static class CollectionExtensions
    {
        public static void ShouldContainOnly(this NameValueCollection collection, params KeyValuePair<string, string> [] valuePairs)
        {
            if( collection.Count != valuePairs.Length)
                throw new SpecificationException(string.Format("The collection should contain {0} items but was {1}", valuePairs.Length, collection.Count));

            foreach (var keyValuePair in valuePairs)
            {
                if(!Equals(keyValuePair.Value, collection[keyValuePair.Key]))
                    throw new SpecificationException(string.Format("The collection should contain key {0} and value {1} but the value was {2}.", keyValuePair.Key, keyValuePair.Value, collection[keyValuePair.Key]));
            }
        }
    }
}
