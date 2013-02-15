using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithoutExecuteMethod
    {
        public Uri BaseUrl { get; set; }
        public string ConfigurationName { get; set; }

        public TestHttpClientBaseTypeWithoutExecuteMethod(Uri baseUrl, string configurationName)
        {
            BaseUrl = baseUrl;
            ConfigurationName = configurationName;
        }
    }
}