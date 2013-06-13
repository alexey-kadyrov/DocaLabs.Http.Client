using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments
    {
        public Uri BaseUrl { get; set; }
        public string ConfigurationName { get; set; }

        public TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
        {
            BaseUrl = baseUrl;
            ConfigurationName = configurationName;
        }

        public TestResultValue Execute(string query)
        {
            return null;
        }
    }
}