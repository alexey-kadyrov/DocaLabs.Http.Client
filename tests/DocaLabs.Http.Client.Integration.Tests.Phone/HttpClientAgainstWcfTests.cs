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
}
