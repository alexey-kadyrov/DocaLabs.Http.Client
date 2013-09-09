using DocaLabs.Http.Client.Integration.Tests._Contract;
using DocaLabs.Http.Client.Integration.Tests._WcfServices;
using Machine.Specifications;
using DataRequest = DocaLabs.Http.Client.Integration.Tests._Contract.DataRequest;
using DataResponse = DocaLabs.Http.Client.Integration.Tests._Contract.DataResponse;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when_getting_http_service_without_any_authentication_which_returns_json_object
    {
        static TestServerHost<TestService> host;
        static ITestGetService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService1>(null, "simpleGetJsonCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_http_service_without_any_authentication_which_returns_json_object
    {
        static TestServerHost<TestService> host;
        static ITestPostJsonService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostJsonService1>(null, "simplePostJsonCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.PostData(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_http_service_without_any_body
    {
        static TestServerHost<TestService> host;
        static ITestEmptyPostService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestEmptyPostService1>(null, "emptyPostCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.EmptyPost(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST EMPTY: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_http_service_without_any_authentication_which_returns_xml_object
    {
        static TestServerHost<TestService> host;
        static ITestGetService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService1>(null, "simpleGetXmlCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET XML: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_http_service_without_any_authentication_which_returns_xml_object
    {
        static TestServerHost<TestService> host;
        static ITestPostXmlService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostXmlService1>(null, "simplePostXmlCall");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.PostData(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST XML: Hello World!");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_http_service_with_headers
    {
        static TestServerHost<TestService> host;
        static ITestGetService1 client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService1>(null, "simpleGetCallWithHeaders");
            host = new TestServerHost<TestService>();
        };

        Because of =
            () => result = client.GetData(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");

        It should_pass_all_headers =
            () => result.Headers.ShouldContain("x-h1: xx-v1", "x-h2: xx-v2");
    }
}
