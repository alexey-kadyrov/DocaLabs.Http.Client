using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseType<TQuery, TResult> : HttpClient<TQuery, TResult>
    {
        public string ExecutionMarker { get; set; }

        public TestHttpClientBaseType(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName)
        {
        }

        protected override TResult ExecutePipeline(TQuery query)
        {
            var result = Activator.CreateInstance<TResult>();

            if(typeof(TQuery) != typeof(VoidType) && typeof(TResult) != typeof(VoidType))
                typeof(TResult).GetProperty("Value").SetValue(result, typeof(TQuery).GetProperty("Value").GetValue(query, null));

            ExecutionMarker = "Pipeline was executed.";

            return result;
        }
    }
}
