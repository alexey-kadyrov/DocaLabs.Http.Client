using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static User result1;
        static User result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_first_task =
            () => result1.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_second_task =
            () => result2.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

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
            Task<User> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_using_canonnical_domain_model_for_input
    {
        static TestServerHost host;
        static CanonicalGetUserRequest request;
        static IGetUserService client;
        static User result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            DefaultRequestBinder.SetModelTransformer(typeof(CanonicalGetUserRequest), c => new GetUserRequest(((CanonicalGetUserRequest)c.OriginalModel).Id));
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new CanonicalGetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of =
            () => result = client.Get(request).Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public class GetUserRequest : GetUser
        {
            public string Format { get; set; }

            public GetUserRequest(Guid id)
            {
                Id = id;
                Format = "json";
            }
        }

        public class CanonicalGetUserRequest
        {
            public Guid Id { get; set; }
        }

        public interface IGetUserService
        {
            Task<User> Get(CanonicalGetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_stream
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static Stream result1;
        static Stream result2;

        Cleanup after_each = () =>
        {
            host.Dispose();
            result1.Dispose();
            result2.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
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
            Task<Stream> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_stream_wrapped_in_rich_response
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static RichResponse<Stream> result1;
        static RichResponse<Stream> result2;

        Cleanup after_each = () =>
        {
            host.Dispose();
            result1.Value.Dispose();
            result2.Value.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1.Value).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_first_task =
            () => result1.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code_for_the_first_task =
            () => result1.Is(HttpStatusCode.OK);

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2.Value).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_second_task =
            () => result2.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code_for_the_second_task =
            () => result2.Is(HttpStatusCode.OK);

        static User ToUser(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
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
            Task<RichResponse<Stream>> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_http_responsestream_wrapped_in_rich_response
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static RichResponse<HttpResponseStream> result1;
        static RichResponse<HttpResponseStream> result2;

        Cleanup after_each = () =>
        {
            host.Dispose();
            result1.Value.Dispose();
            result2.Value.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1.Value).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_first_task =
            () => result1.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code_for_the_first_task =
            () => result1.Is(HttpStatusCode.OK);

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2.Value).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_second_task =
            () => result2.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code_for_the_second_task =
            () => result2.Is(HttpStatusCode.OK);

        static User ToUser(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
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
            Task<RichResponse<HttpResponseStream>> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_http_response_stream
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static HttpResponseStream result1;
        static HttpResponseStream result2;

        Cleanup after_each = () =>
        {
            host.Dispose();
            result1.Dispose();
            result2.Dispose();
        };

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser(HttpResponseStream stream)
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
            Task<HttpResponseStream> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_string
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static string result1;
        static string result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser(string result)
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
            Task<string> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_string_using_palin_text_deserializer
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static string result1;
        static string result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser(string result)
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
            Task<string> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_byte_array
    {
        static TestServerHost host;
        static GetUserRequest request;
        static IGetUserService client;
        static byte[] result1;
        static byte[] result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => ToUser(result1).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => ToUser(result2).ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        static User ToUser(byte[] result)
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
            Task<byte[]> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_using_autogenerated_model
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result1;
        static User result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Users.Data[0].Id), client.Get(Users.Data[0].Id));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => result1.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => result2.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_redirected
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result1;
        static User result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV1");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Users.Data[0].Id), client.Get(Users.Data[0].Id));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => result1.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => result2.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_an_xml_object
    {
        static TestServerHost host;
        static IGetUserService client;
        static User result1;
        static User result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Users.Data[0].Id), client.Get(Users.Data[0].Id));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => result1.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_call_the_service_and_return_data_for_the_second_task =
            () => result2.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "xml");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_and_rich_response_information
    {
        static TestServerHost host;
        static IGetUserService client;
        static RichResponse<User> result1;
        static RichResponse<User> result2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Users.Data[0].Id), client.Get(Users.Data[0].Id));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_data_for_the_first_task =
            () => result1.ShouldMatch(x => x.Value.Id == Users.Data[0].Id && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_first_task =
            () => result1.ETag.ShouldEqual(Users.ETags[Users.Data[0].Id]);

        It should_return_custom_header_for_the_first_task =
            () => result1.Headers["Hello"].ShouldEqual("World!");

        It should_return_200_status_code_for_the_first_task =
            () => result1.Is(HttpStatusCode.OK);

        It should_call_the_service_and_return_data_for_the_second_task =
            () => result2.ShouldMatch(x => x.Value.Id == Users.Data[0].Id && x.Value.FirstName == "John" && x.Value.LastName == "Smith" && x.Value.Email == "john.smith@foo.bar");

        It should_return_etag_for_the_secons_task =
            () => result2.ETag.ShouldEqual(Users.ETags[Users.Data[0].Id]);

        It should_return_custom_header_for_the_second_task =
            () => result2.Headers["Hello"].ShouldEqual("World!");

        It should_return_200_status_code_for_the_second_task =
            () => result2.Is(HttpStatusCode.OK);

        public interface IGetUserService
        {
            Task<RichResponse<User>> Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_conditionally_getting_a_json_object_using_rich_response
    {
        static TestServerHost host;
        static RichRequest<GetUserRequest> request;
        static IGetUserService client;
        static RichResponse<User> result1;
        static RichResponse<User> result2;

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

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            allTask.Wait();

            result1 = allTask.Result[0];
            result2 = allTask.Result[1];
        };

        It should_call_the_service_and_return_null_data_for_the_first_task =
            () => result1.Value.ShouldBeNull();

        It should_return_304_status_code_for_the_first_task =
            () => result1.StatusCode.ShouldEqual(304);

        It should_return_status_description_for_the_first_task =
            () => result1.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header_for_the_first_task =
            () => result1.Headers["Hello"].ShouldEqual("World!");

        It should_call_the_service_and_return_null_data_for_the_second_task =
            () => result2.Value.ShouldBeNull();

        It should_return_304_status_code_for_the_second_task =
            () => result2.StatusCode.ShouldEqual(304);

        It should_return_status_description_for_the_second_task =
            () => result2.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header_for_the_second_task =
            () => result2.Headers["Hello"].ShouldEqual("World!");

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
            Task<RichResponse<User>> Get(RichRequest<GetUserRequest> request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_conditionally_getting_a_json_object_and_not_using_rich_response
    {
        static TestServerHost host;
        static RichRequest<GetUserRequest> request;
        static IGetUserService client;
        static Exception exception1;
        static Exception exception2;

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

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(request), client.Get(request));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            exception1 = aggregateException.InnerExceptions[0];
            exception2 = aggregateException.InnerExceptions[1];
        };

        It should_throw_http_client_web_exception_exception_for_the_first_task =
            () => exception1.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.ShouldNotBeNull();

        It should_return_304_status_code_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.StatusCode.ShouldEqual(304);

        It should_return_status_description_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.Headers["Hello"].ShouldEqual("World!");

        It should_throw_http_client_web_exception_exception_for_the_second_task =
            () => exception2.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.ShouldNotBeNull();

        It should_return_304_status_code_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusCode.ShouldEqual(304);

        It should_return_status_description_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusDescription.ShouldEqual(string.Format("{0} Not Modified", request.Value.Id));

        It should_return_custom_header_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.Headers["Hello"].ShouldEqual("World!");

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
            Task<User> Get(RichRequest<GetUserRequest> request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_receiving_404_for_get_request
    {
        static TestServerHost host;
        static IGetUserService client;
        static Exception exception1;
        static Exception exception2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Guid.Empty), client.Get(Guid.Empty));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            exception1 = aggregateException.InnerExceptions[0];
            exception2 = aggregateException.InnerExceptions[1];
        };

        It should_throw_http_client_web_exception_exception_for_the_first_task =
            () => exception1.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.ShouldNotBeNull();

        It should_return_404_status_code_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.StatusCode.ShouldEqual(404);

        It should_return_status_description_for_the_first_task =
            () => ((HttpClientWebException)exception1).Response.StatusDescription.ShouldEqual("User 00000000-0000-0000-0000-000000000000 does not exist.");

        It should_throw_http_client_web_exception_exception_for_the_seconds_task =
            () => exception2.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.ShouldNotBeNull();

        It should_return_404_status_code_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusCode.ShouldEqual(404);

        It should_return_status_description_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusDescription.ShouldEqual("User 00000000-0000-0000-0000-000000000000 does not exist.");

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_querying_wrong_address
    {
        static TestServerHost host;
        static IGetUserService client;
        static Exception exception1;
        static Exception exception2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("badGetUserrequest");
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var allTask = Task.WhenAll(client.Get(Guid.Empty), client.Get(Guid.Empty));

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            exception1 = aggregateException.InnerExceptions[0];
            exception2 = aggregateException.InnerExceptions[1];
        };

        It should_throw_http_client_web_exception_exception_for_the_first_task =
            () => exception1.ShouldBeOfType<HttpClientWebException>();

        It should_throw_http_client_web_exception_exception_for_the_second_task =
            () => exception2.ShouldBeOfType<HttpClientWebException>();

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "json");
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_one_asynchronous_task_gets_a_json_object_and_another_receives_404
    {
        static TestServerHost host;
        static GetUserRequest request1;
        static GetUserRequest request2;
        static IGetUserService client;
        static User result1;
        static Exception exception2;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            request1 = new GetUserRequest { Id = Users.Data[0].Id };
            request2 = new GetUserRequest { Id = Guid.Empty };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var tasks = new[]
            {
                client.Get(request1), 
                client.Get(request2)
            };
            var allTask = Task.WhenAll(tasks);

            var aggregateException = (AggregateException)Catch.Exception(allTask.Wait);

            result1 = tasks[0].Result;
            exception2 = aggregateException.InnerExceptions[0];
        };

        It should_call_the_service_and_return_data_for_first_task =
            () => result1.ShouldMatch(x => x.Id == request1.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        It should_throw_http_client_web_exception_exception_for_the_seconds_task =
            () => exception2.ShouldBeOfType<HttpClientWebException>();

        It should_return_additional_information_about_the_response_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.ShouldNotBeNull();

        It should_return_404_status_code_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusCode.ShouldEqual(404);

        It should_return_status_description_for_the_second_task =
            () => ((HttpClientWebException)exception2).Response.StatusDescription.ShouldEqual("User 00000000-0000-0000-0000-000000000000 does not exist.");

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
            Task<User> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_putting_a_json_object
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
            () => client.Update(request).Wait();

        It should_call_the_service =
            () => Users.Data[1].ShouldMatch(x => x.Id == request.Id && x.FirstName == "Updated FirstName" && x.LastName == "Updated LastName" && x.Email == "Updated Email");

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            Task Update(UpdateUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_putting_a_json_object_and_getting_rich_response_information
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
            () => result = client.Update(request).Result;

        It should_call_the_service =
            () => Users.Data[1].ShouldMatch(x => x.Id == request.Id && x.FirstName == "Updated FirstName" && x.LastName == "Updated LastName" && x.Email == "Updated Email");

        It should_return_etag =
            () => result.ETag.ShouldEqual(Users.ETags[request.Id]);

        It should_return_200_status_code =
            () => result.Is(HttpStatusCode.OK);

        [SerializeAsJson]
        public interface IUpdateUserService
        {
            Task<RichResponse<VoidType>> Update(UpdateUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object
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
            () => client.Add(request).Wait();

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task Add(AddUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_and_getting_data_back
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
            () => result = client.Add(request).Result;

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        It should_return_the_date =
            () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task<User> Add(AddUserAndReturnDataRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_and_ignoring_retruned_data
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
            () => client.Add(request).Wait();

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == request.Id).ShouldMatch(x => x.Id == request.Id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeAsJson]
        public interface IAddUserService
        {
            Task Add(AddUserAndReturnDataRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_and_ignoring_retruned_data_but_getting_additional_information_about_response
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
            () => result = client.Add(request).Result;

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
            Task<RichResponse<VoidType>> Add(AddUserAndReturnDataRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_conflicting_json_object
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
            () => exception = ((AggregateException)Catch.Exception(() => client.Add(request).Wait())).InnerExceptions[0];


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
            Task Add(AddUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_property
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
            () => client.Add(request).Wait();

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
            Task Add(AddUserRequestEx request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_stream_property
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
            () => client.Add(request).Wait();

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
            Task Add(AddUserRequestEx request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_stream
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
            () => client.Add(request).Wait();

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == id).ShouldMatch(x => x.Id == id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        [SerializeStream(ContentType = "application/json")]
        public interface IAddUserService
        {
            Task Add(Stream request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_posting_a_json_object_using_user_object_as_string
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
            () => client.Add(request).Wait();

        It should_call_the_service =
            () => Users.Data.First(x => x.Id == id).ShouldMatch(x => x.Id == id && x.FirstName == "New FirstName" && x.LastName == "New LastName" && x.Email == "New Email");

        public interface IAddUserService
        {
            Task Add([SerializeAsText(ContentType = "application/json")] string request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_deleting
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
            () => client.Delete(request).Wait();

        It should_call_the_service =
            () => Users.Data.ShouldNotContain(x => x.Id == request.Id);

        public interface IDeleteUserService
        {
            Task Delete(DeleteUserRequest request);
        }
    }

    [Subject(typeof(HttpClient<,>))]
    public class when_asynchronously_executing_request_without_any_parameters_and_return_data
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
            () => client.Delete().Wait();

        It should_call_the_service =
            () => Users.Data.ShouldNotContain(x => x.Id == id);

        public interface IDeleteUserService
        {
            Task Delete();
        }
    }
}
