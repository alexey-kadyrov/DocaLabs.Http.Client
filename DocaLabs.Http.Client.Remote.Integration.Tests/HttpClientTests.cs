using System;
using DocaLabs.Http.Client.Remote.Integration.Tests._Contract;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Remote.Integration.Tests
{
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
            () => result.ShouldMatch(x => x.authenticated && x.user == "first");
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
            () => result.ShouldMatch(x => x.authenticated && x.user == "first");
    }
}
