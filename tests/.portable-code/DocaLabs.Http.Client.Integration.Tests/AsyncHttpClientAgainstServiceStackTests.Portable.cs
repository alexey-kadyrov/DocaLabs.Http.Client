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
    public class when_asynchronously_getting_a_json_object
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static User _result1;
        static User _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_first_task()
        {
            _result1.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_second_task()
        {
            _result2.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<User> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_using_canonnical_domain_model_for_input
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
            _result = _client.ExecuteAsync(_request).Result;
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
            Task<User> ExecuteAsync(CanonicalGetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<CanonicalGetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static Stream _result1;
        static Stream _result2;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result1.Dispose();
            _result2.Dispose();
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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<Stream> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, Stream>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_stream_wrapped_in_rich_response
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static RichResponse<Stream> _result1;
        static RichResponse<Stream> _result2;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result1.Value.Dispose();
            _result2.Value.Dispose();
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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_first_task()
        {
            _result1.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_first_task()
        {
            _result1.Is(HttpStatusCode.OK);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_second_task()
        {
            _result2.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_second_task()
        {
            _result2.Is(HttpStatusCode.OK);
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
            Task<RichResponse<Stream>> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, RichResponse<Stream>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_http_responsestream_wrapped_in_rich_response
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static RichResponse<HttpResponseStream> _result1;
        static RichResponse<HttpResponseStream> _result2;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result1.Value.Dispose();
            _result2.Value.Dispose();
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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_first_task()
        {
            _result1.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_first_task()
        {
            _result1.Is(HttpStatusCode.OK);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2.Value).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_second_task()
        {
            _result2.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_second_task()
        {
            _result2.Is(HttpStatusCode.OK);
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
            Task<RichResponse<HttpResponseStream>> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, RichResponse<HttpResponseStream>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_http_response_stream
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static HttpResponseStream _result1;
        static HttpResponseStream _result2;

        [ClassCleanup]
        public static void Cleanup()
        {
            _result1.Dispose();
            _result2.Dispose();
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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2).ShouldMatch(x => x.Id == 1&& x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<HttpResponseStream> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, HttpResponseStream>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_string
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static string _result1;
        static string _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<string> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, string>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_string_using_palin_text_deserializer
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static string _result1;
        static string _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<string> ExecuteAsync(GetUserRequest request);
        }

        [DeserializeFromPlainText]
        public class GetUserService : AsyncHttpClient<GetUserRequest, string>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_as_byte_array
    {
        static GetUserRequest _request;
        static IGetUserService _client;
        static byte[] _result1;
        static byte[] _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            ToUser(_result1).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            ToUser(_result2).ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
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
            Task<byte[]> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, byte[]>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_redirected
    {
        static IGetUserService _client;
        static User _result1;
        static User _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV1");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(new GetUserRequest { Id = 1 }), _client.ExecuteAsync(new GetUserRequest { Id = 1 }));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            _result1.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            _result2.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            Task<User> ExecuteAsync(GetUserRequest request);
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

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_an_xml_object
    {
        static IGetUserService _client;
        static User _result1;
        static User _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(new GetUserRequest { Id = 1 }), _client.ExecuteAsync(new GetUserRequest { Id = 1 }));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            _result1.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            _result2.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        public interface IGetUserService
        {
            Task<User> ExecuteAsync(GetUserRequest request);
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

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_and_rich_response_information
    {
        static IGetUserService _client;
        static RichResponse<User> _result1;
        static RichResponse<User> _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(new GetUserRequest { Id = 1 }), _client.ExecuteAsync(new GetUserRequest { Id = 1 }));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_first_task()
        {
            _result1.ShouldMatch(x => x.Value.Id == 1 && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_first_task()
        {
            _result1.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_first_task()
        {
            _result1.Headers["Hello"].ShouldEqual("World!");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_first_task()
        {
            _result1.Is(HttpStatusCode.OK);
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_the_second_task()
        {
            _result2.ShouldMatch(x => x.Value.Id == 1 && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_return_etag_for_the_secons_task()
        {
            _result2.ETag.ShouldEqual("i1");
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_second_task()
        {
            _result2.Headers["Hello"].ShouldEqual("World!");
        }

        [TestMethod]
        public void it_should_return_200_status_code_for_the_second_task()
        {
            _result2.Is(HttpStatusCode.OK);
        }

        public interface IGetUserService
        {
            Task<RichResponse<User>> ExecuteAsync(GetUserRequest request);
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

        public class GetUserService : AsyncHttpClient<GetUserRequest, RichResponse<User>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_conditionally_getting_a_json_object_using_rich_response
    {
        static RichRequest<GetUserRequest> _request;
        static IGetUserService _client;
        static RichResponse<User> _result1;
        static RichResponse<User> _result2;

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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            allTask.Wait();

            _result1 = allTask.Result[0];
            _result2 = allTask.Result[1];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_null_data_for_the_first_task()
        {
            _result1.Value.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code_for_the_first_task()
        {
            _result1.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_first_task()
        {
            _result1.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_first_task()
        {
            _result1.Headers["Hello"].ShouldEqual("World!");
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_null_data_for_the_second_task()
        {
            _result2.Value.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code_for_the_second_task()
        {
            _result2.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_second_task()
        {
            _result2.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_second_task()
        {
            _result2.Headers["Hello"].ShouldEqual("World!");
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
            Task<RichResponse<User>> ExecuteAsync(RichRequest<GetUserRequest> request);
        }

        public class GetUserService : AsyncHttpClient<RichRequest<GetUserRequest>, RichResponse<User>>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_conditionally_getting_a_json_object_and_not_using_rich_response
    {
        static RichRequest<GetUserRequest> _request;
        static IGetUserService _client;
        static Exception _exception1;
        static Exception _exception2;

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
            var allTask = Task.WhenAll(_client.ExecuteAsync(_request), _client.ExecuteAsync(_request));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            _exception1 = aggregateException.InnerExceptions[0];
            _exception2 = aggregateException.InnerExceptions[1];

            new ManualResetEvent(false).WaitOne(1000);
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_first_task()
        {
            _exception1.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response_for_the_first_task()
        {
            ((HttpClientWebException) _exception1).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code_for_the_first_task()
        {
            ((HttpClientWebException) _exception1).Response.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_first_task()
        {
            ((HttpClientWebException)_exception1).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_first_task()
        {
            ((HttpClientWebException) _exception1).Response.Headers["Hello"].ShouldEqual("World!");
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_second_task()
        {
            _exception2.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_304_status_code_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.StatusCode.ShouldEqual(304);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_second_task()
        {
            ((HttpClientWebException)_exception2).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", 1));
        }

        [TestMethod]
        public void it_should_return_custom_header_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.Headers["Hello"].ShouldEqual("World!");
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
            Task<User> ExecuteAsync(RichRequest<GetUserRequest> request);
        }

        public class GetUserService : AsyncHttpClient<RichRequest<GetUserRequest>, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_receiving_404_for_get_request
    {
        static IGetUserService _client;
        static Exception _exception1;
        static Exception _exception2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(new GetUserRequest { Id = -1 }), _client.ExecuteAsync(new GetUserRequest { Id = -1 }));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            _exception1 = aggregateException.InnerExceptions[0];
            _exception2 = aggregateException.InnerExceptions[1];
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_first_task()
        {
            _exception1.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response_for_the_first_task()
        {
            ((HttpClientWebException) _exception1).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_404_status_code_for_the_first_task()
        {
            ((HttpClientWebException) _exception1).Response.StatusCode.ShouldEqual(404);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_first_task()
        {
            ((HttpClientWebException)_exception1).Response.StatusDescription.ShouldEqual("User -1 does not exist.");
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_seconds_task()
        {
            _exception2.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_404_status_code_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.StatusCode.ShouldEqual(404);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.StatusDescription.ShouldEqual("User -1 does not exist.");
        }

        public interface IGetUserService
        {
            Task<User> ExecuteAsync(GetUserRequest request);
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

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_querying_wrong_address
    {
        static IGetUserService _client;
        static Exception _exception1;
        static Exception _exception2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("badGetUserrequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.ExecuteAsync(new GetUserRequest { Id = -1 }), _client.ExecuteAsync(new GetUserRequest { Id = -1 }));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            _exception1 = aggregateException.InnerExceptions[0];
            _exception2 = aggregateException.InnerExceptions[1];
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_first_task()
        {
            _exception1.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_second_task()
        {
            _exception2.ShouldBeOfType<HttpClientWebException>();
        }

        public interface IGetUserService
        {
            Task<User> ExecuteAsync(GetUserRequest request);
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

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_one_asynchronous_task_gets_a_json_object_and_another_receives_404
    {
        static GetUserRequest _request1;
        static GetUserRequest _request2;
        static IGetUserService _client;
        static User _result1;
        static Exception _exception2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new GetUserService("getUserV2");
            _request1 = new GetUserRequest { Id = 1 };
            _request2 = new GetUserRequest { Id = -1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var tasks = new[]
            {
                _client.ExecuteAsync(_request1), 
                _client.ExecuteAsync(_request2)
            };
            var allTask = Task.WhenAll(tasks);

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            _result1 = tasks[0].Result;
            _exception2 = aggregateException.InnerExceptions[0];
        }

        [TestMethod]
        public void it_should_call_the_service_and_return_data_for_first_task()
        {
            _result1.ShouldMatch(x => x.Id == 1 && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");
        }

        [TestMethod]
        public void it_should_throw_http_client_web_exception_exception_for_the_seconds_task()
        {
            _exception2.ShouldBeOfType<HttpClientWebException>();
        }

        [TestMethod]
        public void it_should_return_additional_information_about_the_response_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_return_404_status_code_for_the_second_task()
        {
            ((HttpClientWebException) _exception2).Response.StatusCode.ShouldEqual(404);
        }

        [TestMethod]
        public void it_should_return_status_description_for_the_second_task()
        {
            ((HttpClientWebException)_exception2).Response.StatusDescription.ShouldEqual("User -1 does not exist.");
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
            Task<User> ExecuteAsync(GetUserRequest request);
        }

        public class GetUserService : AsyncHttpClient<GetUserRequest, User>, IGetUserService
        {
            public GetUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_putting_a_json_object
    {
        static User _request;
        static IUpdateUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new UpdateUserService("updateUser");
            _request = new User
            {
                Id = 72002,
                FirstName = "Asynchronously Updated FirstName",
                LastName = "Asynchronously Updated LastName",
                Email = "Asynchronously Updated Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var expectedUser = User.GetExpected(72002);
            expectedUser.ShouldMatch(x => x.Id == 72002 && x.FirstName == "Asynchronously Updated FirstName" && x.LastName == "Asynchronously Updated LastName" && x.Email == "Asynchronously Updated Email");
        }

        public interface IUpdateUserService
        {
            Task<VoidType> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class UpdateUserService : AsyncHttpClient<User, VoidType>, IUpdateUserService
        {
            public UpdateUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_putting_a_json_object_and_getting_rich_response_information
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
                Id = 73002,
                FirstName = "Asynchronously Updated FirstName 2",
                LastName = "Asynchronously Updated LastName 2",
                Email = "Asynchronously Updated Email 2"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.ExecuteAsync(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(73002).ShouldMatch(x => x.Id == 73002 && x.FirstName == "Asynchronously Updated FirstName 2" && x.LastName == "Asynchronously Updated LastName 2" && x.Email == "Asynchronously Updated Email 2");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("u73002");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        public interface IUpdateUserService
        {
            Task<RichResponse<VoidType>> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class UpdateUserService : AsyncHttpClient<User, RichResponse<VoidType>>, IUpdateUserService
        {
            public UpdateUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object
    {
        static User _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new AddUserService("addUser");
            _request = new User
            {
                Id = 7142,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7142).ShouldMatch(x => x.Id == 7142 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class AddUserService : AsyncHttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_and_getting_data_back
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
                Id = 7143,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.ExecuteAsync(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7143).ShouldMatch(x => x.Id == 7143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_return_the_data()
        {
            _result.ShouldMatch(x => x.Id == 7143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task<User> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class AddUserService : AsyncHttpClient<User, User>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_and_ignoring_retruned_data
    {
        static User _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EtsblishContext(TestContext context)
        {
            _client = new AddUserService("addUserAndReturnData");
            _request = new User
            {
                Id = 7144,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7144).ShouldMatch(x => x.Id == 7144 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class AddUserService : AsyncHttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_and_ignoring_retruned_data_but_getting_additional_information_about_response
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
                Id = 7145,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.ExecuteAsync(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7145).ShouldMatch(x => x.Id == 7145 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_get_response_status()
        {
            _result.StatusCode.ShouldEqual(200);
        }

        [TestMethod]
        public void it_should_get_etag()
        {
            _result.ETag.ShouldEqual("a7145");
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
            Task<RichResponse<VoidType>> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class AddUserService : AsyncHttpClient<User, RichResponse<VoidType>>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_conflicting_json_object
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
            _exception = ((AggregateException) Catch.Exception(() => _client.ExecuteAsync(_request).Wait())).InnerExceptions[0];
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
            Task<VoidType> ExecuteAsync(User request);
        }

        [SerializeAsJson]
        public class AddUserService : AsyncHttpClient<User, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_property
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
                    Id = 7146,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                }
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7146).ShouldMatch(x => x.Id == 7146 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public User User { get; set; }
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(AddUserRequestEx request);
        }

        public class AddUserService : AsyncHttpClient<AddUserRequestEx, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_stream_property
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
                    Id = 7147,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                })))
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7147).ShouldMatch(x => x.Id == 7147 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }

            [SerializeAsJson]
            public Stream User { get; set; }
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(AddUserRequestEx request);
        }

        public class AddUserService : AsyncHttpClient<AddUserRequestEx, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_stream
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
                Id = 7148,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            })));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7148).ShouldMatch(x => x.Id == 7148 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(Stream request);
        }

        [SerializeStream(ContentType = "application/json")]
        public class AddUserService : AsyncHttpClient<Stream, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_string
    {
        static string _request;
        static IAddUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = new AddUserService("addUser");
            _request = JsonConvert.SerializeObject(new User
            {
                Id = 7149,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.ExecuteAsync(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(7149).ShouldMatch(x => x.Id == 7149 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task<VoidType> ExecuteAsync(string request);
        }

        [SerializeAsText(ContentType = "application/json")]
        public class AddUserService : AsyncHttpClient<string, VoidType>, IAddUserService
        {
            public AddUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }

    [TestClass]
    public class when_asynchronously_deleting
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
            _client.ExecuteAsync(new DeleteUserRequest { Id = 75 }).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => User.GetExpected(75));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            Task<VoidType> ExecuteAsync(DeleteUserRequest id);
        }

        public class DeleteUserRequest
        {
            public long Id { get; set; }
        }

        public class DeleteUserService : AsyncHttpClient<DeleteUserRequest, VoidType>, IDeleteUserService
        {
            public DeleteUserService(string configuration)
                : base(null, configuration)
            {
            }
        }
    }
}
