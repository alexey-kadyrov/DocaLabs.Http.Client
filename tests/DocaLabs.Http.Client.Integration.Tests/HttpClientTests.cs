using System;
using System.Threading;
using DocaLabs.Http.Client.Integration.Tests._Setup;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when
    {
        static ITestService1 client;
        static OutData result;

        Cleanup after_each =
            () => TestServerSetup.Stop();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestService1>(null, "service1");
            TestServerSetup.Start();
        };

        Because of =
            () => result = client.GetData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "Hello World!");
    }

    public interface ITestService1
    {
        OutData GetData(InData query);
    }
}
