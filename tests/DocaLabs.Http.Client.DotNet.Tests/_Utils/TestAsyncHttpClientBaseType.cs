using System;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestAsyncHttpClientBaseType<TInputModel, TOutputModel> : AsyncHttpClient<TInputModel, TOutputModel>
    {
        public string ExecutionMarker { get; set; }

        public TestAsyncHttpClientBaseType(Uri baseUrl, string configurationName)
            : base(baseUrl, configurationName, new AsyncTestExecuteStrategy())
        {
            ((AsyncTestExecuteStrategy) ExecuteStrategy).Client = this;
        }

        class AsyncTestExecuteStrategy : IExecuteStrategy<TInputModel, Task<TOutputModel>>
        {
            // ReSharper disable MemberCanBePrivate.Local
            public TestAsyncHttpClientBaseType<TInputModel, TOutputModel> Client { get; set; }
            // ReSharper restore MemberCanBePrivate.Local

            public Task<TOutputModel> Execute(TInputModel model, Func<TInputModel, Task<TOutputModel>> action)
            {
                var result = Activator.CreateInstance<TOutputModel>();

                if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType) && typeof(TOutputModel).GetProperty("Value") != null)
                {
                    typeof (TOutputModel).GetProperty("Value").SetValue(result, model);
                }

                Client.ExecutionMarker = "Async pipeline was executed.";

                return Task.FromResult(result);
            }
        }
    }
}