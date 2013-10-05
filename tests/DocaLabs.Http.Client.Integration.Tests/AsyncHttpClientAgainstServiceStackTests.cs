using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(AsyncHttpClient<,>))]
    public class when_async_getting_a_json_object
    {
        static TestServerHost host;
        static GetUserRequest request;
        //static IGetUserService client;
        static AsyncHttpClient<GetUserRequest, string> client;
        //static User result;
        static string result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            //client = HttpClientFactory.CreateInstance<IGetUserService>("getUserV2");
            client = new AsyncHttpClient<GetUserRequest, string>(null, "getUserV2");
            request = new GetUserRequest { Id = Users.Data[0].Id };
            host = new TestServerHost();
        };

        Because of = () =>
        {
            var t = client.Execute(request);

            t.ShouldNotBeNull();

            result = t.Result;
        };

        It should_return_data =
            () => result.ShouldNotBeEmpty();

        //It should_call_the_service_and_return_data =
        //    () => result.ShouldMatch(x => x.Id == request.Id && x.FirstName == "John" && x.LastName == "Smith" && x.Email == "john.smith@foo.bar");

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
}
