using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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

        public static void ShouldContainOnly<T>(this IEnumerable<T> actual, params T[] expected)
        {
            CollectionAssert.AreEquivalent(expected, actual);
        }

        public static void ShouldContainOnly(this ICustomKeyValueCollection actual, ICustomKeyValueCollection expected)
        {
            if (actual == null)
                throw new ArgumentNullException("actual");

            if (expected == null)
                throw new ArgumentNullException("expected");

            if (actual.Count != expected.Count)
                throw new AssertionException(string.Format("Expected that collection will have the same number of items but the actual contains {0} and the expected {1}", actual.Count, expected.Count));

            foreach (var key in expected)
            {
                CollectionAssert.Contains(actual, key);
                CollectionAssert.AreEqual(expected.GetValues(key), actual.GetValues(key));
            }
        }

        public static void ShouldBeEmpty(this IEnumerable actual)
        {
            Assert.IsEmpty(actual);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, params T[] expected)
        {
            if (expected == null)
                throw new ArgumentNullException("expected");

            if (expected.Length == 0)
                throw new ArgumentException("Expected collection must have at least one element");

            foreach (var item in expected)
                CollectionAssert.Contains(expected, item);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, Expression<Func<T, bool>> condition)
        {
            var compiledCondition = condition.Compile();
            if (!actual.Any(compiledCondition.Invoke))
                throw new Exception(string.Format("Should contain item matching expression [{0}], but does not.", condition));
        }

        public static void ShouldNotBeEmpty<T>(this T actual) where T : IEnumerable<T>
        {
            Assert.IsNotEmpty(actual);
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
