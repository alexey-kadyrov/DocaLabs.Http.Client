using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DocaLabs.Http.Client.Utils;
using NUnit.Framework;

namespace DocaLabs.Test.Utils
{
    public static class SpecificationExtensions
    {
        public static void ShouldbeTheSameAs<T>(this T actual, T expected)
        {
            Assert.AreSame(expected, actual);
        }

        public static void ShouldContainOnly(this ICustomKeyValueCollection actual, ICustomKeyValueCollection expected)
        {
            if (actual == null)
                throw new ArgumentNullException("actual");

            if (expected == null)
                throw new ArgumentNullException("expected");

            if(actual.Count != expected.Count)
                throw new AssertionException(string.Format("Expected that collection will have the same number of items but the actual contains {0} and the expected {1}", actual.Count, expected.Count));

            foreach (var key in expected)
            {
                CollectionAssert.Contains(actual, key);
                CollectionAssert.AreEqual(expected.GetValues(key), actual.GetValues(key));
            }
        }

        public static void ShouldMatch<T>(this T actual, Expression<Func<T, bool>> condition)
        {
            if (!condition.Compile().Invoke(actual))
                throw new Exception(string.Format("Should match expression [{0}], but does not.", condition));
        }

        public static void ShouldBeOfType<T>(this object obj)
        {
            Assert.IsInstanceOf<T>(obj);
        }

        public static void ShouldBeTrue(this bool value)
        {
            Assert.IsTrue(value);
        }

        public static void ShouldBeFalse(this bool value)
        {
            Assert.IsFalse(value);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, params T[] expected)
        {
            CollectionAssert.IsSubsetOf(expected, actual);
        }

        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldBeNull(this object actual)
        {
            Assert.IsNull(actual);
        }

        public static void ShouldNotBeNull(this object actual)
        {
            Assert.IsNotNull(actual);
        }

        public static void ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
        }
    }
}
