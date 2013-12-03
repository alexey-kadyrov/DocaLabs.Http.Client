using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Integration.Tests
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<User> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new CanonicalGetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Get(_request).Result;
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
            Task<User> Get(CanonicalGetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<Stream> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<RichResponse<Stream>> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<RichResponse<HttpResponseStream>> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<HttpResponseStream> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<string> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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

        [DeserializeFromPlainText]
        public interface IGetUserService
        {
            Task<string> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request = new GetUserRequest { Id = 1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<byte[]> Get(GetUserRequest request);
        }
    }

    [TestClass]
    public class when_asynchronously_getting_a_json_object_using_autogenerated_model
    {
        static IGetUserService _client;
        static User _result1;
        static User _result2;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(1), _client.Get(1));

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
            Task<User> Get(long id, string format = "json");
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(1), _client.Get(1));

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
            Task<User> Get(long id, string format = "json");
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(1), _client.Get(1));

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
            Task<User> Get(long id, string format = "xml");
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(1), _client.Get(1));

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
            Task<RichResponse<User>> Get(long id, string format = "json");
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
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<RichResponse<User>> Get(RichRequest<GetUserRequest> request);
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
            var allTask = Task.WhenAll(_client.Get(_request), _client.Get(_request));

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
            Task<User> Get(RichRequest<GetUserRequest> request);
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(-1), _client.Get(-1));

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
            Task<User> Get(long id, string format = "json");
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("badGetUserrequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            var allTask = Task.WhenAll(_client.Get(-1), _client.Get(-1));

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
            Task<User> Get(long id, string format = "json");
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
            _client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            _request1 = new GetUserRequest { Id = 1 };
            _request2 = new GetUserRequest { Id = -1 };

            BecauseOf();
        }

        static void BecauseOf()
        {
            var tasks = new[]
            {
                _client.Get(_request1), 
                _client.Get(_request2)
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
            Task<User> Get(GetUserRequest request);
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
            _client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            _request = new User
            {
                Id = 2002,
                FirstName = "Asynchronously Updated FirstName",
                LastName = "Asynchronously Updated LastName",
                Email = "Asynchronously Updated Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Update(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var expectedUser = User.GetExpected(2002);
            expectedUser.ShouldMatch(x => x.Id == 2002 && x.FirstName == "Asynchronously Updated FirstName" && x.LastName == "Asynchronously Updated LastName" && x.Email == "Asynchronously Updated Email");
        }

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            Task Update(User request);
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
            _client = HttpClientFactory.CreateInstance<IUpdateUserService>("updateUser");
            _request = new User
            {
                Id = 3002,
                FirstName = "Asynchronously Updated FirstName 2",
                LastName = "Asynchronously Updated LastName 2",
                Email = "Asynchronously Updated Email 2"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Update(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(3002).ShouldMatch(x => x.Id == 3002 && x.FirstName == "Asynchronously Updated FirstName 2" && x.LastName == "Asynchronously Updated LastName 2" && x.Email == "Asynchronously Updated Email 2");
        }

        [TestMethod]
        public void it_should_return_etag()
        {
            _result.ETag.ShouldEqual("u3002");
        }

        [TestMethod]
        public void it_should_return_200_status_code()
        {
            _result.Is(HttpStatusCode.OK);
        }

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            Task<RichResponse<VoidType>> Update(User request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = new User
            {
                Id = 142,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(142).ShouldMatch(x => x.Id == 142 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task Add(User request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 143,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Add(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(143).ShouldMatch(x => x.Id == 143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_return_the_data()
        {
            _result.ShouldMatch(x => x.Id == 143 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task<User> Add(User request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 144,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(144).ShouldMatch(x => x.Id == 144 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task Add(User request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserAndReturnData");
            _request = new User
            {
                Id = 145,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _client.Add(_request).Result;
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(145).ShouldMatch(x => x.Id == 145 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [TestMethod]
        public void it_should_get_response_status()
        {
            _result.StatusCode.ShouldEqual(200);
        }

        [TestMethod]
        public void it_should_get_etag()
        {
            _result.ETag.ShouldEqual("a145");
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
            Task<RichResponse<VoidType>> Add(User request);
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
            _exception = ((AggregateException) Catch.Exception(() => _client.Add(_request).Wait())).InnerExceptions[0];
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
            Task Add(User request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            _request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new User
                {
                    Id = 146,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                }
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(146).ShouldMatch(x => x.Id == 146 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }
            [SerializeAsJson]
            public User User { get; set; }
        }

        public interface IAddUserService
        {
            Task Add(AddUserRequestEx request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUserEx");
            _request = new AddUserRequestEx
            {
                PathPart = "users",
                User = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
                {
                    Id = 147,
                    FirstName = "New FirstName",
                    LastName = "New LastName",
                    Email = "New Email"
                })))
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(147).ShouldMatch(x => x.Id == 147 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public class AddUserRequestEx
        {
            public string PathPart { get; set; }

            [SerializeAsJson]
            public Stream User { get; set; }
        }

        public interface IAddUserService
        {
            Task Add(AddUserRequestEx request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User
            {
                Id = 148,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            })));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(148).ShouldMatch(x => x.Id == 148 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        [SerializeStream(ContentType = "application/json")]
        public interface IAddUserService
        {
            Task Add(Stream request);
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
            _client = HttpClientFactory.CreateInstance<IAddUserService>("addUser");
            _request = JsonConvert.SerializeObject(new User
            {
                Id = 149,
                FirstName = "New FirstName",
                LastName = "New LastName",
                Email = "New Email"
            });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Add(_request).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            User.GetExpected(149).ShouldMatch(x => x.Id == 149 && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");
        }

        public interface IAddUserService
        {
            Task Add([SerializeAsText(ContentType = "application/json")] string request);
        }
    }

    [TestClass]
    public class when_asynchronously_deleting
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
            _client.Delete(5).Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => User.GetExpected(5));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            Task Delete(long id);
        }
    }

    [TestClass]
    public class when_asynchronously_executing_request_without_any_parameters_and_return_data_relying_on_the_url_alone
    {
        static IDeleteUserService _client;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _client = HttpClientFactory.CreateInstance<IDeleteUserService>(new Uri("http://localhost:1337/v2/users/6"), "noParametersRequest");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _client.Delete().Wait();
        }

        [TestMethod]
        public void it_should_call_the_service()
        {
            var exception = Catch.Exception(() => User.GetExpected(6));
            exception.ShouldBeOfType<HttpClientException>();
            exception.Is(HttpStatusCode.NotFound).ShouldBeTrue();
        }

        public interface IDeleteUserService
        {
            Task Delete();
        }
    }
}
