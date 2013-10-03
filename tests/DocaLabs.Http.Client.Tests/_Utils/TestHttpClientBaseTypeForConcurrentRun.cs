using System;
using System.Threading;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> : HttpClient<TInputModel, TOutputModel>
    {
        public string ExecutionMarker { get; set; }

        public TestHttpClientBaseTypeForConcurrentRun(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName, new TestExecuteStrategy())
        {
            ((TestExecuteStrategy)ExecuteStrategy).Client = this;
        }

        class TestExecuteStrategy : IExecuteStrategy<TInputModel, TOutputModel>
        {
            // ReSharper disable MemberCanBePrivate.Local
            public TestHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> Client { get; set; }
            // ReSharper restore MemberCanBePrivate.Local

            public TOutputModel Execute(TInputModel model, Func<TInputModel, TOutputModel> action)
            {
                var result = Activator.CreateInstance<TOutputModel>();

                if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType))
                    typeof(TOutputModel).GetProperty("Value").SetValue(result, typeof(TInputModel).GetProperty("Value").GetValue(model, null));

                Client.ExecutionMarker = "Pipeline was executed.";

                Thread.Sleep(10);

                return result;
            }
        }
    }
}