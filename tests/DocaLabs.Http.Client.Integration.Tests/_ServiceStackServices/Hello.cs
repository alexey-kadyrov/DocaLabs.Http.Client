using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    public class HelloService : Service
    {
        public object Get(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }
}
