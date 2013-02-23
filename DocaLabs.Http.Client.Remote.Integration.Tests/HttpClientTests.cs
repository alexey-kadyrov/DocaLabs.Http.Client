using System;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Authentication;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Get;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Put;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Remote.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when_getting_data
    {
        static IGetData client;
        static GetDataResponse result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IGetData>(new Uri("http://httpbin.org/get"));

        Because of =
            () => result = client.Get(new GetDataRequest { Id = "red" });

        It should_call_the_service_and_return_data =
            () => result.Url.ShouldEqual("http://httpbin.org/get?Id=red");

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_data
    {
        static IPostData client;
        static PostDataResponse result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IPostData>(new Uri("http://httpbin.org/post"));

        Because of =
            () => result = client.Post(new PostDataRequest { Id = 42 });

        It should_call_the_service_and_return_data =
            () => result.Json.Id.ShouldEqual(42);

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_posting_data_mixing_query_string_and_request_body
    {
        static IMixedPostData client;
        static MixedPostDataResponse result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IMixedPostData>(new Uri("http://httpbin.org/post"));

        Because of = () => result = client.Post(new MixedPostDataRequest
        {
            Id = 42,
            Data = new InnerPostDataRequest
            {
                Value = "Hello World!"
            }
        });

        It should_call_the_service_and_return_data =
            () => result.Json.Value.ShouldEqual("Hello World!");

        It should_pass_data_in_query_string =
            () => result.Url.ShouldEqual("http://httpbin.org/post?Id=42");

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_puting_data
    {
        static IPutData client;
        static PutDataResponse result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IPutData>(null, "put");

        Because of =
            () => result = client.Post(new PutDataRequest { Id = 42 });

        It should_call_the_service_and_return_data =
            () => result.Json.Id.ShouldEqual(42);

        It should_pass_accept_encoding_header =
            () => result.Headers.Keys.ShouldContain(x => string.Compare(x, "accept-encoding", StringComparison.OrdinalIgnoreCase) == 0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_using_basic_http_authentication
    {
        static IAuthenticatedUser client;
        static AuthenticatedUser result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(null, "basicUserAuthentication");

        Because of =
            () => result = client.Get();

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_using_basic_http_authentication_over_https
    {
        static IAuthenticatedUser client;
        static AuthenticatedUser result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(new Uri("https://httpbin.org/basic-auth/first/passwd42"), "basicUserAuthentication");

        Because of =
            () => result = client.Get();

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_using_digest_http_authentication
    {
        static IAuthenticatedUser client;
        static AuthenticatedUser result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(null, "digestAuthentication");

        Because of =
            () => result = client.Get();

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_using_digest_http_authentication_over_https
    {
        static IAuthenticatedUser client;
        static AuthenticatedUser result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IAuthenticatedUser>(new Uri("https://httpbin.org/digest-auth/qop/first/passwd42"), "digestAuthentication");

        Because of =
            () => result = client.Get();

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Authenticated && x.User == "first");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_getting_google_over_https
    {
        static IGoogleSearch client;
        static string result;

        Establish context =
            () => client = HttpClientFactory.CreateInstance<IGoogleSearch>(new Uri("https://www.google.com/"));

        Because of =
            () => result = client.GetPage();

        It should_call_the_service_and_return_data =
            () => result.ShouldNotBeEmpty();
    }
}
