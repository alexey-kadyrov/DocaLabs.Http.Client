using System;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object
    {
        static TestServerHost<IGetUserService> host;
        static GetUser request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>(new Uri("http://localhost:1337/users/{id}?format=json"));
            request = new GetUser { Id = Guid.NewGuid() };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(GetUser request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_an_xml_object
    {
        static TestServerHost<IGetUserService> host;
        static GetUser request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>(new Uri("http://localhost:1337/users/{id}?format=xml"));
            request = new GetUser { Id = Guid.NewGuid() };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(GetUser request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_receiving_404_for_get_request
    {
        static TestServerHost<IGetUserService> host;
        static IGetUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>(new Uri("http://localhost:1337/users/{id}?format=xml"));
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(new GetUser { Id = Guid.Empty }));

        It should_throw_http_client_exception_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_return_additional_information_about_the_response =
            () => ((HttpClientException) exception).Response.ShouldNotBeNull();

        It should_return_404_status_code =
            () => ((HttpClientException) exception).Response.StatusCode.ShouldEqual(404);

        It should_return_status_description =
            () => ((HttpClientException)exception).Response.StatusDescription.ShouldEqual("User 00000000-0000-0000-0000-000000000000 does not exist.");

        public interface IGetUserService
        {
            User Get(GetUser request);
        }
    }
}
