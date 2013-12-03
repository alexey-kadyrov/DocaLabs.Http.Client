using System;
using System.Threading;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Authentication;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Get;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Put;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Remote.Integration.Tests
{
    [TestClass]
    public class when_getting_data
    {
        static IGetData _client;
        static GetDataResponse _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IGetData>(new Uri("http://httpbin.org/get"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(new GetDataRequest {Id = "red"});
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.Url.ShouldEqual("http://httpbin.org/get?Id=red");
        }

        [TestMethod]
        public void it_should_pass_accept_encoding_header()
        {
            _result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

    [TestClass]
    public class when_posting_data
    {
        static IPostData _client;
        static PostDataResponse _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IPostData>(new Uri("http://httpbin.org/post"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Post(new PostDataRequest {Id = 42});
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.Json.Id.ShouldEqual(42);
        }

        [TestMethod]
        public void it_should_pass_accept_encoding_header()
        {
            _result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

    [TestClass]
    public class when_posting_data_mixing_query_string_and_request_body
    {
        static IMixedPostData _client;
        static MixedPostDataResponse _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IMixedPostData>(new Uri("http://httpbin.org/post"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Post(new MixedPostDataRequest
            {
                Id = 42,
                Data = new InnerPostDataRequest
                {
                    Value = "Hello World!"
                }
            });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.Json.Value.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_pass_data_in_query_string()
        {
            _result.Url.ShouldEqual("http://httpbin.org/post?Id=42");
        }

        [TestMethod]
        public void it_should_pass_accept_encoding_header()
        {
            _result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

    [TestClass]
    public class when_puting_data
    {
        static IPutData _client;
        static PutDataResponse _result;

        [ClassInitialize]
        public static void EtsbalishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IPutData>(null, "put");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Post(new PutDataRequest {Id = 42});
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.Json.Id.ShouldEqual(42);
        }

        [TestMethod]
        public void it_should_pass_accept_encoding_header()
        {
            _result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
        }
    }

    [TestClass]
    public class when_getting_using_basic_http_authentication
    {
        static IAuthenticatedUser _client;
        static AuthenticatedUser _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(null, "basicUserAuthentication");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get();
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Authenticated && x.User == "first");
        }
    }

    [TestClass]
    public class when_getting_using_basic_http_authentication_over_https
    {
        static IAuthenticatedUser _client;
        static AuthenticatedUser _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(new Uri("https://httpbin.org/basic-auth/first/passwd42"), "basicUserAuthentication");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get();
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Authenticated && x.User == "first");
        }
    }

    [TestClass]
    public class when_getting_using_digest_http_authentication
    {
        static IAuthenticatedUser _client;
        static AuthenticatedUser _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(null, "digestAuthentication");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get();
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Authenticated && x.User == "first");
        }
    }

    [TestClass]
    public class when_getting_using_digest_http_authentication_over_https
    {
        static IAuthenticatedUser _client;
        static AuthenticatedUser _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(new Uri("https://httpbin.org/digest-auth/qop/first/passwd42"), "digestAuthentication");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get();
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Authenticated && x.User == "first");
        }
    }

    [TestClass]
    public class when_getting_google_over_https
    {
        static IGoogleSearch _client;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            _client = HttpClientFactory.CreateInstance<IGoogleSearch>(new Uri("https://www.google.com/"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.GetPage();
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldNotBeEmpty();
        }
    }
}
