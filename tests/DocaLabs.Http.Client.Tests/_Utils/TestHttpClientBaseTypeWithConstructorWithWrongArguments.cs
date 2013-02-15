using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithConstructorWithWrongArguments : HttpClient<TestsQuery, TestResult>
    {
        public TestHttpClientBaseTypeWithConstructorWithWrongArguments(string configurationName)
            : base(new Uri("http://foo.bar/"), configurationName)
        {
        }
    }
}