using DocaLabs.Http.Client.Integration.Tests._Service;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_without_any_authentication_which_returns_json_object
    {
        static TestServerHost<TestService> host;
        static ITestService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestService1>(null, "simpleJsonCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_without_any_authentication_which_returns_xml_object
    {
        static TestServerHost<TestService> host;
        static ITestService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestService1>(null, "simpleXmlCall");
            host = new TestServerHost<TestService>();
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
