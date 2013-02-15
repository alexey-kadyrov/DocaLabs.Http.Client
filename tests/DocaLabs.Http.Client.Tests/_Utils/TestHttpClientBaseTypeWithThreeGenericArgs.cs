using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithThreeGenericArgs<TQuery, TResult, TSomeOther> : HttpClient<TQuery, TResult>
    {
        public TestHttpClientBaseTypeWithThreeGenericArgs(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName)
        {
        }

        public TSomeOther SomeMethod()
        {
            return default(TSomeOther);
        }
    }
}