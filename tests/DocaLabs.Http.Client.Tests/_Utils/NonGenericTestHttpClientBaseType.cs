using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class NonGenericTestHttpClientBaseType : TestHttpClientBaseType<TestsQuery, TestResult>
    {
        public NonGenericTestHttpClientBaseType(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName)
        {
        }
    }
}