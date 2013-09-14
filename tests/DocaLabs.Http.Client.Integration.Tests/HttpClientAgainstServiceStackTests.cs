using System;
using System.Net;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object
    {
        static TestServerHost<IGetUserService> host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_redirected
    {
        static TestServerHost<IGetUserService> host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_an_xml_object
    {
        static TestServerHost<IGetUserService> host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id, Format = "xml" };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_and_rich_response_information
    {
        static TestServerHost<IGetUserService> host;
        static GetUserRequest request;
        static IGetUserService client;
        static RichResponse<User> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value.Id == request.Id && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");

        It should_return_etag =
            () => result.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_custom_header =
            () => result.Headers["Hello"].ShouldEqual("World!");

        It should_return_200_status_code =
            () => result.Is(HttpStatusCode.OK);

        public interface IGetUserService
        {
            RichResponse<User> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_conditionally_getting_a_json_object_using_rich_response
    {
        static TestServerHost<IGetUserService> host;
        static RichRequest<GetUserRequest> request;
        static IGetUserService client;
        static RichResponse<User> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = Users.Data[0].Id },
                IfNoneMatch = Users.ETags[Users.Data[0].Id]
            };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_null_data =
            () => result.Value.ShouldBeNull();

        It should_return_304_status_code =
            () => result.StatusCode.ShouldEqual(304);

        It should_return_status_description =
            () => result.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header =
            () => result.Headers["Hello"].ShouldEqual("World!");

        public interface IGetUserService
        {
            RichResponse<User> Get(RichRequest<GetUserRequest> request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_conditionally_getting_a_json_object_and_not_using_rich_response
    {
        static TestServerHost<IGetUserService> host;
        static RichRequest<GetUserRequest> request;
        static IGetUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = Users.Data[0].Id },
                IfNoneMatch = Users.ETags[Users.Data[0].Id]
            };
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(request));


        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response =
            () => ((HttpClientWebException)exception).Response.ShouldNotBeNull();

        It should_return_304_status_code =
            () => ((HttpClientWebException)exception).Response.StatusCode.ShouldEqual(304);

        It should_return_status_description =
            () => ((HttpClientWebException)exception).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header =
            () => ((HttpClientWebException)exception).Response.Headers["Hello"].ShouldEqual("World!");

        public interface IGetUserService
        {
            User Get(RichRequest<GetUserRequest> request);
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
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(new GetUserRequest { Id = Guid.Empty }));

        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response =
            () => ((HttpClientWebException)exception).Response.ShouldNotBeNull();

        It should_return_404_status_code =
            () => ((HttpClientWebException)exception).Response.StatusCode.ShouldEqual(404);

        It should_return_status_description =
            () => ((HttpClientWebException)exception).Response.StatusDescription.ShouldEqual("User 00000000-0000-0000-0000-000000000000 does not exist.");

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_querying_wrong_address
    {
        static TestServerHost<IGetUserService> host;
        static IGetUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("badGetUserrequest");
            host = new TestServerHost<IGetUserService>();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(new GetUserRequest { Id = Guid.Empty }));

        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }
}
