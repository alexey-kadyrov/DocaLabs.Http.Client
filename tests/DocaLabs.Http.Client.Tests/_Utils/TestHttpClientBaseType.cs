using System;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseType<TInputModel, TOutputModel> : HttpClient<TInputModel, TOutputModel>
    {
        public TInputModel Model { get; set; }
        public string ExecutionMarker { get; set; }

        public TestHttpClientBaseType(Uri baseUrl, string configurationName, TInputModel model)
            : base(baseUrl, configurationName, new TestExecuteStrategy())
        {
            Model = model;
            ((TestExecuteStrategy) ExecuteStrategy).Client = this;
        }

        class TestExecuteStrategy : IExecuteStrategy<TOutputModel>
        {
            public TestHttpClientBaseType<TInputModel, TOutputModel> Client { get; set; }

            public TOutputModel Execute(Func<TOutputModel> action)
            {
                var result = Activator.CreateInstance<TOutputModel>();

                if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType))
                    typeof(TOutputModel).GetProperty("Value").SetValue(result, typeof(TInputModel).GetProperty("Value").GetValue(Client.Model, null));

                Client.ExecutionMarker = "Pipeline was executed.";

                return result;
            }
        }
    }
}
