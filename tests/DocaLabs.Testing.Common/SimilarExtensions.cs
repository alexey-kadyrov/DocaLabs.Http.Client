using Machine.Specifications;

namespace DocaLabs.Testing.Common
{
    /// <remarks>
    /// It's impossible to redefine the equality behaviour for objects as it will mess the entity tracking
    /// in the DataServiceContext which uses Dictionary and expects the default behaviour for
    /// reference types - comparing references.
    /// The dictionary uses hash code first to assign bucket and if there are more than one object 
    /// with the same hash it will compare those objects.
    /// If the equality is redefined the way that it compares properties of the object the DataServiceContext
    /// won't be able to detect that it's already tracking the entity as the dictionary will look for the
    /// changed object in different bucket.
    /// By convention if the Equals is redefined the GetHaskCode must be redefined as well in the way
    /// that it must always return the same value for equal objects.
    /// The good article is: http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx
    /// </remarks>
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
    }
}
