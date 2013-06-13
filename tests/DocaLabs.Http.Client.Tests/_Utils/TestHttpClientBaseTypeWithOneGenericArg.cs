using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithOneGenericArg<TQuery> : HttpClient<TQuery, TestResultValue>
    {
        public TestHttpClientBaseTypeWithOneGenericArg(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName)
        {
        }
    }
}