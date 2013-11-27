using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet
{
    [TestClass]
    public class when_getting_a_json_object
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public class GetUserRequest
        {
            public long Id { get; set; }

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

    [TestClass]
    public class when_getting_a_json_object_using_canonnical_domain_model_for_input
    {
        static CanonicalGetUserRequest _request;
        static IGetUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new CanonicalGetUserRequest { UserId = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            DefaultRequestBinder.SetModelTransformer(typeof(CanonicalGetUserRequest), c => new GetUserRequest(((CanonicalGetUserRequest)c.OriginalModel).UserId));

            _result = _client.Get(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public class GetUserRequest
        {
            public long Id { get; set; }

            public string Format { get; set; }

            public GetUserRequest(long id)
            {
                Id = id;
                Format = "json";
            }
        }

        public class CanonicalGetUserRequest
        {
            public long UserId { get; set; }
        }

        public interface IGetUserService
        {
            User Get(CanonicalGetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static Stream _stream;

        [ClassCleanup]
        public static void Cleanup()
        {
            _stream.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _stream = _client.Get(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            StreamToUser(_stream).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User StreamToUser(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
            {
                return JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
            }
        }

        public class GetUserRequest
        {
            public long Id { get; set; }

            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            Stream Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_stream_wrapped_in_rich_response
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static RichResponse<Stream> _result;

        [ClassCleanup]
        public void Cleanup()
        {
            _result.Value.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            StreamToUser(_result.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        static User StreamToUser(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
            {
                return JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
            }
        }

        public class GetUserRequest
        {
            public long Id { get; set; }

            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        public interface IGetUserService
        {
            RichResponse<Stream> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_as_http_response_stream
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static HttpResponseStream stream;

        Cleanup after_each = () =>
        {
            host.Dispose();
            stream.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of =
            () => stream = client.Get(request);

        It should_call_the_service_and_return_data =
            () => ToUser().ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser()
        {
            return JsonConvert.DeserializeObject<User>(stream.AsString());
        }

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
            HttpResponseStream Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_as_http_response_stream_wrapped_in_rich_response
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static RichResponse<HttpResponseStream> result;

        Cleanup after_each = () =>
        {
            host.Dispose();
            result.Value.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(request);

        It should_call_the_service_and_return_data =
            () => ToUser().ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_return_etag =
            () => result.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code =
            () => result.Is(HttpStatusCode.OK);

        static User ToUser()
        {
            using (var reader = new StreamReader(result.Value, Encoding.UTF8, true, 4096, true))
            {
                return JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
            }
        }

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
            RichResponse<HttpResponseStream> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_as_string
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static string result;

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
            () => ToUser().ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser()
        {
            return JsonConvert.DeserializeObject<User>(result);
        }

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
            string Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_as_string_using_palin_text_deserializer
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static string result;

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
            () => ToUser().ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser()
        {
            return JsonConvert.DeserializeObject<User>(result);
        }

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "json";
            }
        }

        [DeserializeFromPlainText]
        public interface IGetUserService
        {
            string Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_as_byte_array
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static byte[] result;

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
            () => ToUser().ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser()
        {
            return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(result));
        }

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
            byte[] Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_using_autogenerated_model
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(Users.Data[0].Id);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_redirected
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(Users.Data[0].Id);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_an_xml_object
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(Users.Data[0].Id);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            User Get(Guid id, string format = "xml");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_getting_a_json_object_and_rich_response_information
    {
        static TestServerHost host;
        static IGetUserService client;
        static RichResponse<User> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(Users.Data[0].Id);

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Value.Id == Users.Data[0].Id && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");

        It should_return_etag =
            () => result.ETag.ShouldEqual(Users.ETags[Users.Data[0].Id]);

        It should_return_custom_header =
            () => result.Headers["Hello"].ShouldEqual("World!");

        It should_return_200_status_code =
            () => result.Is(HttpStatusCode.OK);

        public interface IGetUserService
        {
            RichResponse<User> Get(Guid id, string format = "json");
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
            () => exception = Catch.Exception(() => client.Get(Guid.Empty));

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
            User Get(Guid id, string format = "json");
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
            () => exception = Catch.Exception(() => client.Get(Guid.Empty));

        It should_throw_http_client_web_exception_exception =
            () => exception.ShouldBeOfType<HttpClientWebException>();

        public interface IGetUserService
        {
            User Get(Guid id, string format = "json");
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
    public class when_posting_a_json_object_and_getting_data_back
    {
        static TestServerHost host;
        static AddUserAndReturnDataRequest request;
        static IAddUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            request = new AddUserAndReturnDataRequest
            {
                Id = Guid.NewGuid(),
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        It should_return_the_date =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeAsJson]
        public interface IAddUserService
        {
            User Add(AddUserAndReturnDataRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object_and_ignoring_retruned_data
    {
        static TestServerHost host;
        static AddUserAndReturnDataRequest request;
        static IAddUserService client;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            request = new AddUserAndReturnDataRequest
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
            void Add(AddUserAndReturnDataRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object_and_ignoring_retruned_data_but_getting_additional_information_about_response
    {
        static TestServerHost host;
        static AddUserAndReturnDataRequest request;
        static IAddUserService client;
        static RichResponse<VoidType> result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            request = new AddUserAndReturnDataRequest
            {
                Id = Guid.NewGuid(),
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        It should_get_response_status =
            () => result.StatusCode.ShouldEqual(200);

        It should_get_etag =
            () => result.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_get_content_type =
            () => result.ContentType.ShouldBeEqualIgnoringCase("application/json");

        It should_get_status_description =
            () => result.StatusDescription.ShouldBeEqualIgnoringCase("OK");

        [SerializeAsJson]
        public interface IAddUserService
        {
            RichResponse<VoidType> Add(AddUserAndReturnDataRequest request);
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
    public class when_posting_a_json_object_using_user_object_as_property
    {
        static TestServerHost host;
        static AddUserRequestEx request;
        static IAddUserService client;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new AddUserRequest
                {
                    Id = Guid.NewGuid(),
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                }
            };
            host = new TestServerHost();
        };

        Because of =
            () => client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.User.Id).ShouldMatch(x => x.Id == request.User.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public AddUserRequest User { get; set; }
        }

        public interface IAddUserService
        {
            void Add(AddUserRequestEx request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object_using_user_object_as_stream_property
    {
        static TestServerHost host;
        static Guid id;
        static AddUserRequestEx request;
        static IAddUserService client;

        Cleanup after_each = () =>
        {
            request.User.Dispose();
            host.Dispose();
        };

        Establish context = () =>
        {
            id = Guid.NewGuid();
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new AddUserRequest
                {
                    Id = id,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                })))
            };
            host = new TestServerHost();
        };

        Because of =
            () => client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == id).ShouldMatch(x => x.Id == id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public Stream User { get; set; }
        }

        public interface IAddUserService
        {
            void Add(AddUserRequestEx request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object_using_user_object_as_stream
    {
        static TestServerHost host;
        static Guid id;
        static Stream request;
        static IAddUserService client;

        Cleanup after_each = () =>
        {
            request.Dispose();
            host.Dispose();
        };

        Establish context = () =>
        {
            id = Guid.NewGuid();
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            request = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new AddUserRequest
            {
                Id = id,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            })));
            host = new TestServerHost();
        };

        Because of =
            () => client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == id).ShouldMatch(x => x.Id == id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeStream(ContentType = "application/json")]
        public interface IAddUserService
        {
            void Add(Stream request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_posting_a_json_object_using_user_object_as_string
    {
        static TestServerHost host;
        static Guid id;
        static string request;
        static IAddUserService client;

        Cleanup after_each = 
            () => host.Dispose();

        Establish context = () =>
        {
            id = Guid.NewGuid();
            client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            request = JsonConvert.SerializeObject(new AddUserRequest
            {
                Id = id,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            });
            host = new TestServerHost();
        };

        Because of =
            () => client.Add(request);

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == id).ShouldMatch(x => x.Id == id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        public interface IAddUserService
        {
            void Add([SerializeAsText(ContentType = "application/json")] string request);
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

    [Subject(typeof(HttpClient<,>))]
    public class when_executing_request_without_any_parameters_and_return_data
    {
        static TestServerHost host;
        static IDeleteUserService client;
        static Guid id;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            id = Users.Data[2].Id;
            client = HttpClientFactory.CreateInstance<IDeleteUserService>(new Uri(string.Format("http://localhost:1337/v2/users/{0}", id)), "noParametersRequest");
            host = new TestServerHost();
        };

        Because of =
            () => client.Delete();

        It should_call_the_service =
            () => Users.Data.ShouldNotContain(x => x.Id == id);

        public interface IDeleteUserService
        {
            void Delete();
        }
    }
}
