using DocaLabs.Http.Client.Integration.Tests._Service;
using DocaLabs.Http.Client.RequestSerialization;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when_getting_http_service_without_any_authentication_which_returns_json_object
    {
        static TestServerHost<TestService> host;
        static ITestGetService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService1>(null, "simpleGetJsonCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_http_service_without_any_authentication_which_returns_json_object
    {
        static TestServerHost<TestService> host;
        static ITestPostJsonService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostJsonService1>(null, "simplePostJsonCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.PostData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_http_service_without_any_authentication_which_returns_xml_object
    {
        static TestServerHost<TestService> host;
        static ITestGetService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService1>(null, "simpleGetXmlCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET XML: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_http_service_without_any_authentication_which_returns_xml_object
    {
        static TestServerHost<TestService> host;
        static ITestPostXmlService1 client;
        static OutData result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostXmlService1>(null, "simplePostXmlCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.PostData(new InData { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST XML: Hello World!");
    }

    public interface ITestGetService1
    {
        OutData GetData(InData query);
    }

    [SerializeAsJson]
    public interface ITestPostJsonService1
    {
        OutData PostData(InData query);
    }

    [SerializeAsXml]
    public interface ITestPostXmlService1
    {
        OutData PostData(InData query);
    }
}
