using System;
using System.Collections.Specialized;
using Machine.Specifications;

namespace DocaLabs.Testing.Common
{
    public static class SimilarExtensions
    {
        public static void ShouldBeSimilar<TActual, TExpected>(this TActual actual, TExpected expected)
        {
            if (Equals(actual, null))
            {
                if(Equals(expected, null))
                    return;

                throw new SpecificationException(string.Format("Expected {0} to be null.", expected));
            }

            if (Equals(expected, null))
                throw new SpecificationException(string.Format("Expected {0} to be null.", actual));

            foreach (var expectedProperty in expected.GetType().GetProperties())
            {
                var actualProperty = actual.GetType().GetProperty(expectedProperty.Name);
                if(actualProperty == null)
                    throw new SpecificationException(string.Format("Expected that the object should support {0} property.", expectedProperty.Name));

                var actualValue = actualProperty.GetValue(actual, null);
                var expectedValue = expectedProperty.GetValue(expected, null);

                if(!Equals(actualValue, expectedValue))
                    throw new SpecificationException(string.Format("Expected {0} should be equal to {1} but was {2}.", actualProperty.Name, expectedValue, actualValue));
            }
        }
   
        public static void ShouldContainOnly(this NameValueCollection actual, NameValueCollection expected)
        {
            if (actual == null)
                throw new ArgumentNullException("actual");

            if (expected == null)
                throw new ArgumentNullException("expected");

            if(actual.Count != expected.Count)
                throw new SpecificationException(string.Format("Expected that collection will have the same number of items but the actual contains {0} and the expected {1}", actual.Count, expected.Count));

            foreach (string key in expected)
            {
                actual.ShouldContain(key);
                actual[key].ShouldEqual(expected[key]);
            }
        }
    }
}
