using System;
using DocaLabs.Http.Client.Integration.Tests._ServiceStackServices;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [Subject(typeof(HttpClient<,>))]
    public class when_getting_http_service
    {
        static TestServerHost<HelloService> host;
        static IHelloService client;
        static HelloResponse result;

        Cleanup after_each =
            () => host.Dispose();

        Establish context = () =>
        {
            client = HttpClientFactory.CreateInstance<IHelloService>(new Uri("http://localhost:1337/HelloService/{name}?format=json"));
            host = new TestServerHost<HelloService>();
        };

        Because of =
            () => result = client.Get(new Hello { Name = "Hello World!" });

        It should_call_the_service_and_return_data =
            () => result.Result.ShouldEqual("Hello, " + "Hello World!");

        public interface IHelloService
        {
            HelloResponse Get(Hello request);
        }
    }
}
