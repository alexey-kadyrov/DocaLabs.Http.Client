using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocaLabs.Http.Client.Utils;
using NUnit.Framework;

namespace DocaLabs.Test.Utils
{
    public static class CollectionSpecificationExtensions
    {
        public static void ShouldContainOnly(this ICustomKeyValueCollection collection, params NameValue[] valuePairs)
        {
            Assert.AreEqual(valuePairs.Length, collection.Count, "The collection should contain the specified number of items.");

            foreach (var keyValuePair in valuePairs)
                CollectionAssert.AreEqual(keyValuePair.Value, collection.GetValues(keyValuePair.Name), "The collection should contain the same values.");
        }
    }

    public class NameValue
    {
        public string Name { get; private set; }
        public IReadOnlyList<string> Value { get; private set; }

        public NameValue(string name, string value)
        {
            Name = name;
            Value = new ReadOnlyCollection<string>(new[] { value });
        }
    }
}
