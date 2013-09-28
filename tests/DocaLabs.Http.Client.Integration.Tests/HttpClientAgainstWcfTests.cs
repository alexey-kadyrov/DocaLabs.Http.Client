using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Integration.Tests._Contract;
using DocaLabs.Http.Client.Integration.Tests._Utils;
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
    class when_posting_to_http_service_without_any_authentication_which_returns_json_object
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
    class when_posting_to_http_service_without_any_body
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
    class when_posting_to_http_service_without_any_authentication_which_returns_xml_object
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
    public class when_getting_http_service_with_basic_authentication_which_returns_json_object
    {
        static TestServerHost<TestServiceWithBasicCredentials> host;
        static ITestGetService client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService>(null, "basicAuthenticationGet");
            host = new TestServerHost<TestServiceWithBasicCredentials>();
        };

        Because of =
            () => result = client.Get(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");

        public interface ITestGetService
        {
            DataResponse Get(DataRequest query);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_http_service_with_basic_authentication_with_wrong_credentials
    {
        static TestServerHost<TestServiceWithBasicCredentials> host;
        static ITestGetService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestGetService>(null, "basicAuthenticationGetWithWrongCredentials");
            host = new TestServerHost<TestServiceWithBasicCredentials>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(new DataRequest { Value1 = 42, Value2 = "Hello World!" }));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_return_forbiden_status_code =
            () => exception.Is(HttpStatusCode.Forbidden).ShouldBeTrue();

        public interface ITestGetService
        {
            DataResponse Get(DataRequest query);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_to_http_service_with_basic_authentication_which_returns_json_object
    {
        static TestServerHost<TestServiceWithBasicCredentials> host;
        static ITestPostService client;
        static DataResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostService>(null, "basicAuthenticationPost");
            host = new TestServerHost<TestServiceWithBasicCredentials>();
        };

        Because of =
            () => result = client.Post(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");

        [SerializeAsJson]
        public interface ITestPostService
        {
            DataResponse Post(DataRequest query);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_to_http_service_with_basic_authentication_with_wrong_credentials
    {
        static TestServerHost<TestServiceWithBasicCredentials> host;
        static ITestPostService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<ITestPostService>(null, "basicAuthenticationPostWithWrongCredentials");
            host = new TestServerHost<TestServiceWithBasicCredentials>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Post(new DataRequest { Value1 = 42, Value2 = "Hello World!" }));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_return_forbiden_status_code =
            () => exception.Is(HttpStatusCode.Forbidden).ShouldBeTrue();

        [SerializeAsJson]
        public interface ITestPostService
        {
            DataResponse Post(DataRequest query);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_http_service_with_certificate_authentication_which_returns_json_object
    {
        static TestServerHost<TestServiceWithCertificate> host;
        static ITestGetService client;
        static DataResponse result;
        static X509Certificate2 ca_root_certificate;
        static X509Certificate2 localhost_certificate;
        static X509Certificate2 client_certificate;

        Cleanup after_each = () =>
        {
            host.Dispose();

            client_certificate.Uninstall(StoreName.TrustedPeople, StoreLocation.LocalMachine);
            localhost_certificate.Uninstall(StoreName.My, StoreLocation.LocalMachine);
            ca_root_certificate.Uninstall(StoreName.Root, StoreLocation.LocalMachine);

            CertificateUtils.UnbindPort(5705);
        };

        Establish context = () =>
        {
            ca_root_certificate = CertificateUtils.Install("MyCA.cer", StoreName.Root, StoreLocation.LocalMachine);
            localhost_certificate = CertificateUtils.Install("localhost.pfx", StoreName.My, StoreLocation.LocalMachine);
            client_certificate = CertificateUtils.Install("client.pfx", StoreName.TrustedPeople, StoreLocation.LocalMachine);

            localhost_certificate.BindToPort(5705);

            client = HttpClientFactory.CreateInstance<ITestGetService>(null, "certificateAuthenticationGet");
            host = new TestServerHost<TestServiceWithCertificate>();
        };

        Because of =
            () => result = client.Get(new DataRequest { Value1 = 42, Value2 = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");

        public interface ITestGetService
        {
            DataResponse Get(DataRequest query);
        }
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
