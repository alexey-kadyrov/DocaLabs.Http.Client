using System;
using System.Threading;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeForConcurrentRun<TQuery, TResult> : HttpClient<TQuery, TResult>
    {
        public string ExecutionMarker { get; set; }

        public TestHttpClientBaseTypeForConcurrentRun(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName)
        {
        }

        protected override TResult ExecutePipeline(TQuery model)
        {
            var result = Activator.CreateInstance<TResult>();

            if(typeof(TQuery) != typeof(VoidType) && typeof(TResult) != typeof(VoidType))
                typeof(TResult).GetProperty("Value").SetValue(result, typeof(TQuery).GetProperty("Value").GetValue(model, null));

            ExecutionMarker = "Pipeline was executed.";

            Thread.Sleep(10);

            return result;
        }
    }
}