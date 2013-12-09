using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace DocaLabs.Http.Client.Integration.Tests.Phone
{
    [TestClass]
    public class when_getting_http_service_without_any_authentication_which_returns_json_object
    {
        static IService _client;
        static Response _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new Service("simpleGetJsonCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration) 
                : base(null, configuration)
            {
            }
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
            _client = new Service("simplePostJsonCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        [SerializeAsJson]
        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("emptyPostCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST EMPTY: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("simpleGetXmlCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET XML: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("simplePostXmlCall");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST XML: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        [SerializeAsXml]
        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("basicAuthenticationGet");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "GET JSON: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("basicAuthenticationGetWithWrongCredentials");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" }));
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
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("basicAuthenticationPost");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value1 == 42 && x.Value2 == "POST JSON: Hello World!");
        }

        public interface IService
        {
            Response Execute(Request query);
        }

        [SerializeAsJson]
        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("basicAuthenticationPostWithWrongCredentials");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" }));
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
            Response Execute(Request query);
        }

        [SerializeAsJson]
        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new Service("simpleGetCallWithHeaders");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new Request { Value1 = 42, Value2 = "Hello World!" });
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
            Response Execute(Request query);
        }

        public class Service : HttpClient<Request, Response>, IService
        {
            public Service(string configuration)
                : base(null, configuration)
            {
            }
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
