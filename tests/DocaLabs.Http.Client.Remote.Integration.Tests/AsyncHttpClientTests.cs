using System;
using System.Threading;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Authentication;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Get;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Put;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Remote.Integration.Tests
{
    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_data
    {
        static IGetDataAsync client;
        static GetDataResponse result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IGetDataAsync>(new Uri("http://httpbin.org/get"));
        };

        Because of =
            () => result = client.Get(new GetDataRequest { Id = "red" }).Result;

        It should_call_the_service_and_return_data =
            () => result.Url.ShouldEqual("http://httpbin.org/get?Id=red");

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_posting_data
    {
        static IPostDataAsync client;
        static PostDataResponse result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IPostDataAsync>(new Uri("http://httpbin.org/post"));
        };

        Because of =
            () => result = client.Post(new PostDataRequest { Id = 42 }).Result;

        It should_call_the_service_and_return_data =
            () => result.Json.Id.ShouldEqual(42);

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_posting_data_mixing_query_string_and_request_body
    {
        static IMixedPostDataAsync client;
        static MixedPostDataResponse result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IMixedPostDataAsync>(new Uri("http://httpbin.org/post"));
        };

        Because of = () => result = client.Post(new MixedPostDataRequest
        {
            Id = 42,
            Data = new InnerPostDataRequest
            {
                Value = "Hello World!"
            }
        }).Result;

        It should_call_the_service_and_return_data =
            () => result.Json.Value.ShouldEqual("Hello World!");

        It should_pass_data_in_query_string =
            () => result.Url.ShouldEqual("http://httpbin.org/post?Id=42");

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_puting_data
    {
        static IPutDataAsync client;
        static PutDataResponse result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IPutDataAsync>(null, "put");
        };

        Because of =
            () => result = client.Post(new PutDataRequest { Id = 42 }).Result;

        It should_call_the_service_and_return_data =
            () => result.Json.Id.ShouldEqual(42);

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_using_basic_http_authentication
    {
        static IAuthenticatedUserAsync client;
        static AuthenticatedUser result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IAuthenticatedUserAsync>(null, "basicUserAuthentication");
        };

        Because of =
            () => result = client.Get().Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_using_basic_http_authentication_over_https
    {
        static IAuthenticatedUserAsync client;
        static AuthenticatedUser result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IAuthenticatedUserAsync>(new Uri("https://httpbin.org/basic-auth/first/passwd42"), "basicUserAuthentication");
        };

        Because of =
            () => result = client.Get().Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_using_digest_http_authentication
    {
        static IAuthenticatedUserAsync client;
        static AuthenticatedUser result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IAuthenticatedUserAsync>(null, "digestAuthentication");
        };

        Because of =
            () => result = client.Get().Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_using_digest_http_authentication_over_https
    {
        static IAuthenticatedUserAsync client;
        static AuthenticatedUser result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IAuthenticatedUserAsync>(new Uri("https://httpbin.org/digest-auth/qop/first/passwd42"), "digestAuthentication");
        };

        Because of =
            () => result = client.Get().Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_getting_google_over_https
    {
        static IGoogleSearchAsync client;
        static string result;

        Establish context = () =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(1)); // to be gentle on the remote endpoint, once in second is enough
            client = HttpClientFactory.CreateInstance<IGoogleSearchAsync>(new Uri("https://www.google.com/"));
        };

        Because of =
            () => result = client.GetPage().Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldNotBeEmpty();
    }
}
