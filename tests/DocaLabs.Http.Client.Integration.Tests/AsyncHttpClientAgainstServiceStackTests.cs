using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            () => result = client.Get(request).Result;

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
            Task<User> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_string
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
            () => result = client.Get(request).Result;

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
            Task<string> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_string_using_palin_text_deserializer
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
            () => result = client.Get(request).Result;

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
            Task<string> Get(GetUserRequest request);
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_an_xml_object
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
            () => result = client.Get(Users.Data[0].Id).Result;

        It should_call_the_service_and_return_data =
            () => result.ShouldMatch(x => x.Id == Users.Data[0].Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

        public interface IGetUserService
        {
            Task<User> Get(Guid id, string format = "xml");
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_byte_array
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
            () => result = client.Get(request).Result;

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
            Task<byte[]> Get(GetUserRequest request);
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
}
