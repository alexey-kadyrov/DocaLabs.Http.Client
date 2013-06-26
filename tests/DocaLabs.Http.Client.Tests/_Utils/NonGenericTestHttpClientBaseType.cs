using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class NonGenericTestHttpClientBaseType : TestHttpClientBaseType<TestsQuery, TestResultValue>
    {
        public NonGenericTestHttpClientBaseType(Uri baseUrl, string configurationName, TestsQuery model)
            : base(baseUrl, configurationName, model)
        {
        }
    }
}