using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Integration.Tests;
using DocaLabs.Test.Utils;
#if GENERIC_DOT_NET
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Integration.Portable.Tests
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
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
            User Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            DefaultRequestBinder.SetModelTransformer(typeof(CanonicalGetUserRequest), c => new GetUserRequest(((CanonicalGetUserRequest)c.OriginalModel).Id));
            _client = new GetUserService("getUserV2");
            _request = new CanonicalGetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
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
            public long Id { get; set; }
        }

        public interface IGetUserService
        {
            User Execute(CanonicalGetUserRequest request);
        }

        public class GetUserService : HttpClient<CanonicalGetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static Stream _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(Stream stream)
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
            Stream Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, Stream>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            ToUser(_result.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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

        static User ToUser(Stream stream)
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
            RichResponse<Stream> Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, RichResponse<Stream>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_http_responsestream_wrapped_in_rich_response
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            ToUser(_result.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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

        static User ToUser(Stream stream)
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
            RichResponse<HttpResponseStream> Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, RichResponse<HttpResponseStream>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_getting_a_json_object_as_http_response_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static HttpResponseStream _result;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(HttpResponseStream stream)
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
            HttpResponseStream Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, HttpResponseStream>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
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
            string Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, string>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
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
            string Execute(GetUserRequest request);
        }

        [DeserializeFromPlainText]
        public class GetUserService : HttpClient<GetUserRequest, string>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            ToUser(_result).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        static User ToUser(byte[] result)
        {
            return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(result, 0, result.Length));
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
            byte[] Execute(GetUserRequest request);
        }

        public class GetUserService : HttpClient<GetUserRequest, byte[]>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV1");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new GetUserRequest { Id = 1 });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            User Execute(GetUserRequest request);
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

        public class GetUserService : HttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new GetUserRequest { Id = 1 });
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data()
        {
            _result.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            User Execute(GetUserRequest request);
        }

        public class GetUserRequest
        {
            public long Id { get; set; }

            public string Format { get; set; }

            public GetUserRequest()
            {
                Format = "xml";
            }
        }

        public class GetUserService : HttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(new GetUserRequest { Id = 1 });
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
            RichResponse<User> Execute(GetUserRequest request);
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

        public class GetUserService : HttpClient<GetUserRequest, RichResponse<User>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = 1 },
                IfNoneMatch = "i1"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
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
            _result.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
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
            RichResponse<User> Execute(RichRequest<GetUserRequest> request);
        }

        public class GetUserService : HttpClient<RichRequest<GetUserRequest>, RichResponse<User>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");
            _request = new RichRequest<GetUserRequest>
            {
                Value = new GetUserRequest { Id = 1 },
                IfNoneMatch = "i1"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Execute(_request));
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
            ((HttpClientWebException)_exception).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
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
            User Execute(RichRequest<GetUserRequest> request);
        }

        public class GetUserService : HttpClient<RichRequest<GetUserRequest>, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Execute(new GetUserRequest { Id = -1 }));
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
            ((HttpClientWebException)_exception).Response.StatusDescription.ShouldEqual("User -1 does not exist.");
        }

        public interface IGetUserService
        {
            User Execute(GetUserRequest request);
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

        public class GetUserService : HttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new GetUserService("badGetUserrequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _client.Execute(new GetUserRequest { Id = -1 }));
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception()
        {
            _exception.ShouldBeOfType<HttpClientWebException>();
        }

        public interface IGetUserService
        {
            User Execute(GetUserRequest request);
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

        public class GetUserService : HttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_putting_a_json_object
    {
        static User _request;
        static IUpdateUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new UpdateUserService("updateUser");
            _request = new User
            {
                Id = 82002,
                FirstName = "Asynchronously Updated FirstName",
                LastName = "Asynchronously Updated LastName",
                Email = "Asynchronously Updated Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var expectedUser = User.GetExpected(82002);
            expectedUser.ShouldMatch(x => x.Id == 82002 && x.FirstName == "Asynchronously Updated FirstName" && x.LastName == "Asynchronously Updated LastName" && x.Email == "Asynchronously Updated Email");
        }

        public interface IUpdateUserService
        {
            VoidType Execute(User request);
        }

        [SerializeAsJson]
        public class UpdateUserService : HttpClient<User, VoidType>, IUpdateUserService
        {
            public UpdateUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new UpdateUserService("updateUser");
            _request = new User
            {
                Id = 83002,
                FirstName = "Asynchronously Updated FirstName 2",
                LastName = "Asynchronously Updated LastName 2",
                Email = "Asynchronously Updated Email 2"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(83002).ShouldMatch(x => x.Id == 83002 && x.FirstName == "Asynchronously Updated FirstName 2" && x.LastName == "Asynchronously Updated LastName 2" && x.Email == "Asynchronously Updated Email 2");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("u83002");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        public interface IUpdateUserService
        {
            RichResponse<VoidType> Execute(User request);
        }

        [SerializeAsJson]
        public class UpdateUserService : HttpClient<User, RichResponse<VoidType>>, IUpdateUserService
        {
            public UpdateUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUser");
            _request = new User
            {
                Id = 8142,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8142).ShouldMatch(x => x.Id == 8142 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            VoidType Execute(User request);
        }

        [SerializeAsJson]
        public class AddUserService : HttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUserAndReturnData");
            _request = new User
            {
                Id = 8143,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8143).ShouldMatch(x => x.Id == 8143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_return_the_data()
        {
            _result.ShouldMatch(x => x.Id == 8143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            User Execute(User request);
        }

        [SerializeAsJson]
        public class AddUserService : HttpClient<User, User>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_posting_a_json_object_and_ignoring_retruned_data
    {
        static User _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EtsblishContext(TestContext context)
        {
            _client = new AddUserService("addUserAndReturnData");
            _request = new User
            {
                Id = 8144,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8144).ShouldMatch(x => x.Id == 8144 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            VoidType Execute(User request);
        }

        [SerializeAsJson]
        public class AddUserService : HttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUserAndReturnData");
            _request = new User
            {
                Id = 8145,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8145).ShouldMatch(x => x.Id == 8145 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_get_response_status()
        {
            _result.StatusCode.ShouldEqual(200);
        }

        [TestMethod]
        public void it_should_get_etag()
        {
            _result.ETag.ShouldEqual("a8145");
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

        public interface IAddUserService
        {
            RichResponse<VoidType> Execute(User request);
        }

        [SerializeAsJson]
        public class AddUserService : HttpClient<User, RichResponse<VoidType>>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUser");
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
            _exception = Catch.Exception(() => _client.Execute(_request));
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

        public interface IAddUserService
        {
            VoidType Execute(User request);
        }

        [SerializeAsJson]
        public class AddUserService : HttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_posting_a_json_object_using_user_object_as_property
    {
        static AddUserRequestEx _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new AddUserService("addUserEx");
            _request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new User
                {
                    Id = 8146,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                }
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8146).ShouldMatch(x => x.Id == 8146 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public User User { get; set; }
        }

        public interface IAddUserService
        {
            VoidType Execute(AddUserRequestEx request);
        }

        public class AddUserService : HttpClient<AddUserRequestEx, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUserEx");
            _request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
                {
                    Id = 8147,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                })))
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8147).ShouldMatch(x => x.Id == 8147 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }

            [SerializeAsJson]
            public Stream User { get; set; }
        }

        public interface IAddUserService
        {
            VoidType Execute(AddUserRequestEx request);
        }

        public class AddUserService : HttpClient<AddUserRequestEx, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUser");
            _request = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
            {
                Id = 8148,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            })));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8148).ShouldMatch(x => x.Id == 8148 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            VoidType Execute(Stream request);
        }

        [SerializeStream(ContentType = "application/json")]
        public class AddUserService : HttpClient<Stream, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
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
            _client = new AddUserService("addUser");
            _request = JsonConvert.SerializeObject(new User
            {
                Id = 8149,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(_request);
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(8149).ShouldMatch(x => x.Id == 8149 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            VoidType Execute(string request);
        }

        [SerializeAsText(ContentType = "application/json")]
        public class AddUserService : HttpClient<string, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_deleting
    {
        static IDeleteUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new DeleteUserService("deleteUser");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Execute(new DeleteUserRequest { Id = 85 });
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => User.GetExpected(85));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            VoidType Execute(DeleteUserRequest id);
        }

        public class DeleteUserRequest
        {
            public long Id { get; set; }
        }

        public class DeleteUserService : HttpClient<DeleteUserRequest, VoidType>, IDeleteUserService
        {
            public DeleteUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }
}
