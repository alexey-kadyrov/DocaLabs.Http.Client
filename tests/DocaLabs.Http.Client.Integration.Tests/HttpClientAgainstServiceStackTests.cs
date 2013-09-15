﻿using System;
using System.Linq;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_redirected
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_an_xml_object
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id, Format = "xml" };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_and_rich_response_information
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static RichResponse<User> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
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

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            RichResponse<User> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_conditionally_getting_a_json_object_using_rich_response
    {
        static TestServerHost host;
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
            host = new TestServerHost();
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

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            RichResponse<User> Get(RichRequest<GetUserRequest> request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_conditionally_getting_a_json_object_and_not_using_rich_response
    {
        static TestServerHost host;
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
            host = new TestServerHost();
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

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(RichRequest<GetUserRequest> request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_receiving_404_for_get_request
    {
        static TestServerHost host;
        static IGetUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
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

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_querying_wrong_address
    {
        static TestServerHost host;
        static IGetUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("badGetUserrequest");
            host = new TestServerHost();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Get(new GetUserRequest { Id = Guid.Empty }));

        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            User Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_putting_a_json_object
    {
        static TestServerHost host;
        static UpdateUserRequest request;
        static IUpdateUserService client;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            request = new UpdateUserRequest
            {
                Id = Users.Data[1].Id,
                FirstName = "Updated FirstName",
                LastName = "Updated LastName",
                Email = "Updated Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => client.Update(request);

        It should_call_the_service =
            () => Users.Data[1].ShouldMatch(x => x.Id == request.Id && x.FirstName == "Updated FirstName" && x.LastName == "Updated LastName" && x.Email == "Updated Email");

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            void Update(UpdateUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_putting_a_json_object_and_getting_rich_response_information
    {
        static TestServerHost host;
        static UpdateUserRequest request;
        static IUpdateUserService client;
        static RichResponse<VoidType> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            request = new UpdateUserRequest
            {
                Id = Users.Data[1].Id,
                FirstName = "Updated FirstName",
                LastName = "Updated LastName",
                Email = "Updated Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Update(request);

        It should_call_the_service =
            () => Users.Data[1].ShouldMatch(x => x.Id == request.Id && x.FirstName == "Updated FirstName" && x.LastName == "Updated LastName" && x.Email == "Updated Email");

        It should_return_etag =
            () => result.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code =
            () => result.Is(HttpStatusCode.OK);

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            RichResponse<VoidType> Update(UpdateUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object
    {
        static TestServerHost host;
        static AddUserRequest request;
        static IAddUserService client;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            request = new AddUserRequest
            {
                Id = Guid.NewGuid(),
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeAsJson]
        public interface IAddUserService
        {
            void Add(AddUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_conflicting_json_object
    {
        static TestServerHost host;
        static AddUserRequest request;
        static IAddUserService client;
        static Exception exception;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            request = new AddUserRequest
            {
                Id = Users.Data[1].Id,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => exception = Catch.Exception(() => client.Add(request));


        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response =
            () => ((HttpClientWebException)exception).Response.ShouldNotBeNull();

        It should_return_409_status_code =
            () => ((HttpClientWebException)exception).Response.StatusCode.ShouldEqual(409);

        It should_return_status_description =
            () => ((HttpClientWebException)exception).Response.StatusDescription.ShouldEqual(string.Format("User {0} already exist.", request.Id));

        [SerializeAsJson]
        public interface IAddUserService
        {
            void Add(AddUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_deleting
    {
        static TestServerHost host;
        static DeleteUserRequest request;
        static IDeleteUserService client;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IDeleteUserService>("deleteUser");
            request = new DeleteUserRequest { Id = Users.Data[2].Id };
            host = new TestServerHost();
        };

        Because of =
            () => client.Delete(request);

        It should_call_the_service =
            () => Users.Data.ShouldNotContain(x => x.Id == request.Id);

        public interface IDeleteUserService
        {
            void Delete(DeleteUserRequest request);
        }
    }
}
