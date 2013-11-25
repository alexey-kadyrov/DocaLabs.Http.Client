using System.Collections.Specialized;
using Machine.Specifications;

namespace DocaLabs.Testing.Common
{
    public static class CollectionSpecificationExtensions
    {
        public static void ShouldContainOnly(this NameValueCollection collection, params NameValue[] valuePairs)
        {
            if( collection.Count != valuePairs.Length)
                throw new SpecificationException(string.Format("The collection should contain {0} items but was {1}", valuePairs.Length, collection.Count));

            foreach (var keyValuePair in valuePairs)
            {
                if(!Equals(keyValuePair.Value, collection[keyValuePair.Name]))
                    throw new SpecificationException(string.Format("The collection should contain name {0} and value {1} but the value was {2}.", keyValuePair.Name, keyValuePair.Value, collection[keyValuePair.Name]));
            }
        }
    }

    public class NameValue
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
