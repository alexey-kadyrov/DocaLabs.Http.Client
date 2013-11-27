using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Integration.Tests.DotNet._Contract;
using DocaLabs.Test.Utils;
using DocaLabs.Test.Utils.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet
{
    [TestClass]
    public class when_getting_http_service_without_any_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "simpleGetJsonCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.GetData(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        public interface IService
        {
            Response GetData(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_posting_to_http_service_without_any_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "simplePostJsonCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.PostData(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
        }

        [SerializeAsJson]
        public interface IService
        {
            Response PostData(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_posting_to_http_service_without_any_body
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "emptyPostCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.EmptyPost(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST EMPTY: Hello World!");
        }
        
        public interface IService
        {
            Response EmptyPost(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_getting_http_service_without_any_authentication_which_returns_xml_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "simpleGetXmlCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.GetData(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET XML: Hello World!");
        }

        public interface IService
        {
            Response GetData(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        [XmlRoot(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_posting_to_http_service_without_any_authentication_which_returns_xml_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstbalishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "simplePostXmlCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.PostData(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST XML: Hello World!");
        }

        [SerializeAsXml]
        public interface IService
        {
            Response PostData(Request query);
        }

        [XmlRoot(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        [XmlRoot(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_getting_http_service_with_basic_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "basicAuthenticationGet");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        public interface IService
        {
            Response Get(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_getting_http_service_with_basic_authentication_with_wrong_credentials
    {
        static IService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "basicAuthenticationGetWithWrongCredentials");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Get(new Request { Value1 = 42, Value2 = "Hello World!" }));
        }

        [TestMethod]
        public void it_should_throw_http_client_exception()
        {
            _exception.ShouldBeOfType<HttpClientException>();
        }

        [TestMethod]
        public void it_should_return_forbiden_status_code()
        {
            _exception.Is(HttpStatusCode.Forbidden).ShouldBeTrue();
        }

        public interface IService
        {
            Response Get(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_posting_to_http_service_with_basic_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "basicAuthenticationPost");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Post(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
        }

        [SerializeAsJson]
        public interface IService
        {
            Response Post(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_posting_to_http_service_with_basic_authentication_with_wrong_credentials
    {
        static IService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "basicAuthenticationPostWithWrongCredentials");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Post(new DataRequest { Value1 = 42, Value2 = "Hello World!" }));
        }

        [TestMethod]
        public void it_should_throw_http_client_exception()
        {
            _exception.ShouldBeOfType<HttpClientException>();
        }

        [TestMethod]
        public void it_should_return_forbiden_status_code()
        {
            _exception.Is(HttpStatusCode.Forbidden).ShouldBeTrue();
        }

        [SerializeAsJson]
        public interface IService
        {
            DataResponse Post(DataRequest query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_getting_http_service_with_certificate_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;
        static X509Certificate2 _clientCertificate;

        [ClassCleanup]
        public static void Cleanup()
        {
            _clientCertificate.Uninstall(StoreName.TrustedPeople, StoreLocation.LocalMachine);
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _clientCertificate = CertificateUtils.Install("client.pfx", StoreName.TrustedPeople, StoreLocation.LocalMachine);
            _client = HttpClientFactory.CreateInstance<IService>(null, "certificateAuthenticationGet");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        public interface IService
        {
            Response Get(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }

    [TestClass]
    public class when_getting_http_service_with_headers
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IService>(null, "simpleGetCallWithHeaders");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.GetData(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        [TestMethod]
        public void it_should_pass_all_headers()
        {
            _result.Headers.ShouldContain("x-h1: xx-v1", "x-h2: xx-v2");
        }
        public interface IService
        {
            Response GetData(Request query);
        }

        public class Request
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        public class Response
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public string[] Headers { get; set; }
        }
    }
}
