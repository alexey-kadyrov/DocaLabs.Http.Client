using System.Threading.Tasks;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_asynchronously_getting_a_json_object_as_string
    {
        static TestServerHost host;
        static GetUserRequest request;
        static AsyncHttpClient<GetUserRequest, string> client;
        static string result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = new AsyncHttpClient<GetUserRequest, string>(null, "getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = 
            () => result = client.Execute(request).Result;

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
}
