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
        public static void Cleanup()
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

    [TestClass]
    public class when_getting_a_json_object_as_http_response_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static HttpResponseStream _stream;

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

        static User StreamToUser(HttpResponseStreamCore stream)
        {
            return JsonConvert.DeserializeObject<User>(stream.AsString());
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
            HttpResponseStream Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_http_response_stream_wrapped_in_rich_response
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static RichResponse<HttpResponseStream> _result;

        [ClassCleanup]
        public static void Cleanup()
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
            RichResponse<HttpResponseStream> Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_string
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static string _result;

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
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(string result)
        {
            return JsonConvert.DeserializeObject<User>(result);
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
            string Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_string_using_palin_text_deserializer
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static string _result;

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
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(string result)
        {
            return JsonConvert.DeserializeObject<User>(result);
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

        [DeserializeFromPlainText]
        public interface IGetUserService
        {
            string Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_byte_array
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static byte[] _result;

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
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(byte[] result)
        {
            return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(result));
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
            byte[] Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_getting_a_json_object_using_autogenerated_model
    {
        static IGetUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(1);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }
        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_redirected
    {
        static IGetUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(1);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_getting_an_xml_object
    {
        static IGetUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(1);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "xml");
        }
    }

    [TestClass]
    public class when_getting_a_json_object_and_rich_response_information
    {
        static IGetUserService _client;
        static RichResponse<User> _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(1);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Value.Id == 1 && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_custom_header()
        {
            _result.Headers["Hello"].ShouldEqual("World!");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        public interface IGetUserService
        {
            RichResponse<User> Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_conditionally_getting_a_json_object_using_rich_response
    {
        static RichRequest<GetUserRequest> _request;
        static IGetUserService _client;
        static RichResponse<User> _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = 1 },
                IfNoneMatch = "i1"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_null_data()
        {
            _result.Value.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code()
        {
            _result.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description()
        {
            _result.StatusDescription.ShouldEqual("1 Not Modified");
        }

        [TestMethod]
        public void it_should_return_custom_header()
        {
            _result.Headers["Hello"].ShouldEqual("World!");
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
            RichResponse<User> Get(RichRequest<GetUserRequest> request);
        }
    }

    [TestClass]
    public class when_conditionally_getting_a_json_object_and_not_using_rich_response
    {
        static RichRequest<GetUserRequest> _request;
        static IGetUserService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = 1 },
                IfNoneMatch = "i1"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Get(_request));
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception()
        {
            _exception.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response()
        {
            ((HttpClientWebException) _exception).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code()
        {
            ((HttpClientWebException) _exception).Response.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description()
        {
            ((HttpClientWebException)_exception).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", _request.Value.Id));
        }

        [TestMethod]
        public void it_should_return_custom_header()
        {
            ((HttpClientWebException) _exception).Response.Headers["Hello"].ShouldEqual("World!");
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
            User Get(RichRequest<GetUserRequest> request);
        }
    }

    [TestClass]
    public class when_receiving_404_for_get_request
    {
        static IGetUserService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Get(42));
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception()
        {
            _exception.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response()
        {
            ((HttpClientWebException) _exception).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_404_status_code()
        {
            ((HttpClientWebException) _exception).Response.StatusCode.ShouldEqual(404);
        }

        [TestMethod]
        public void it_should_return_status_description()
        {
            ((HttpClientWebException)_exception).Response.StatusDescription.ShouldEqual("User 42 does not exist.");
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_querying_wrong_address
    {
        static IGetUserService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("badGetUserrequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Get(1));
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception()
        {
            _exception.ShouldBeOfType<HttpClientWebException>();
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_putting_a_json_object
    {
        static User _request;
        static IUpdateUserService _client;

        [ClassInitialize]
        public static void EstablisContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            _request = new User
            {
                Id = 2,
                FirstName = "Updated FirstName",
                LastName = "Updated LastName",
                Email = "Updated Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Update(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var updatedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(2);
            updatedUser.ShouldMatch(x => x.Id == _request.Id && x.FirstName == "Updated FirstName" && x.LastName == "Updated LastName" && x.Email == "Updated Email");
        }

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            void Update(User request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_putting_a_json_object_and_getting_rich_response_information
    {
        static User _request;
        static IUpdateUserService _client;
        static RichResponse<VoidType> _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            _request = new User
            {
                Id = 2,
                FirstName = "Updated FirstName 2",
                LastName = "Updated LastName 2",
                Email = "Updated Email 2"
            };

            BecuaseOf();
        }

        static void BecuaseOf()
        {
            _result = _client.Update(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var updatedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(2);

            updatedUser.ShouldMatch(x => x.Id == _request.Id && x.FirstName == "Updated FirstName 2" && x.LastName == "Updated LastName 2" && x.Email == "Updated Email 2");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("u2");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            RichResponse<VoidType> Update(User request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object
    {
        static User _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = new User
            {
                Id = 11,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(11);
            addedUser.ShouldMatch(x => x.Id == 11 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            void Add(User request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object_and_getting_data_back
    {
        static User _request;
        static IAddUserService _client;
        static User _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 12,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_the_data()
        {
            _result.ShouldMatch(x => x.Id == 12 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            User Add(User request);
        }
    }

    [TestClass]
    public class when_posting_a_json_object_and_ignoring_retruned_data
    {
        static User _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 13,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(13);
            addedUser.ShouldMatch(x => x.Id == 13 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            void Add(User request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object_and_ignoring_retruned_data_but_getting_additional_information_about_response
    {
        static User _request;
        static IAddUserService _client;
        static RichResponse<VoidType> _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 14,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(14);
            addedUser.ShouldMatch(x => x.Id == 14 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_get_response_status()
        {
            _result.StatusCode.ShouldEqual(200);
        }

        [TestMethod]
        public void it_should_get_etag()
        {
            _result.ETag.ShouldEqual("a14");
        }

        [TestMethod]
        public void it_should_get_content_type()
        {
            _result.ContentType.ShouldBeEqualIgnoringCase("application/json");
        }

        [TestMethod]
        public void it_should_get_status_description()
        {
            _result.StatusDescription.ShouldBeEqualIgnoringCase("OK");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            RichResponse<VoidType> Add(User request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_conflicting_json_object
    {
        static User _request;
        static IAddUserService _client;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = new User
            {
                Id = 2,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Add(_request));
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception()
        {
            _exception.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response()
        {
            ((HttpClientWebException) _exception).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_409_status_code()
        {
            ((HttpClientWebException) _exception).Response.StatusCode.ShouldEqual(409);
        }

        [TestMethod]
        public void it_should_return_status_description()
        {
            ((HttpClientWebException)_exception).Response.StatusDescription.ShouldEqual(string.Format("User {0} already exist.", _request.Id));
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            void Add(User request);
        }
    }

    [TestClass]
    public class when_posting_a_json_object_using_user_object_as_property
    {
        static AddUserRequest _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            _request = new AddUserRequest
            {
                PathPart = "users",
                User = new User
                {
                    Id = 77,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                }
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(77);
            addedUser.ShouldMatch(x => x.Id == 77 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequest
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public User User { get; set; }
        }

        public interface IAddUserService
        {
            void Add(AddUserRequest request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object_using_user_object_as_stream_property
    {
        static AddUserRequestEx _request;
        static IAddUserService _client;

        [ClassCleanup]
        public static void Cleanup()
        {
            _request.User.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            _request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
                {
                    Id = 78,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                })))
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(78);
            addedUser.ShouldMatch(x => x.Id == 78 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

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

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object_using_user_object_as_stream
    {
        static Stream _request;
        static IAddUserService _client;

        [ClassCleanup]
        public static void Cleanup()
        {
            _request.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
            {
                Id = 79,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            })));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(79);
            addedUser.ShouldMatch(x => x.Id == 79 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeStream(ContentType = "application/json")]
        public interface IAddUserService
        {
            void Add(Stream request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_posting_a_json_object_using_user_object_as_string
    {
        static string _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = JsonConvert.SerializeObject(new User
            {
                Id = 80,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var addedUser = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(80);
            addedUser.ShouldMatch(x => x.Id == 80 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            void Add([SerializeAsText(ContentType = "application/json")] string request);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_deleting
    {
        static IDeleteUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IDeleteUserService>("deleteUser");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Delete(3);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(3));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            void Delete(long id);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }

    [TestClass]
    public class when_executing_request_without_any_parameters_and_return_data_relying_on_provided_url
    {
        static IDeleteUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IDeleteUserService>(new Uri(string.Format("http://localhost:1337/v2/users/{0}", 4)), "noParametersRequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Delete();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(4));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            void Delete();
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }
}
