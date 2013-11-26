using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Test.Services._WcfServices
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TestServiceWithBasicCredentials : ITestService2
    {
        public Response Get(int value1, string value2)
        {
            return new Response
            {
                Value1 = value1,
                Value2 = "GET JSON: " + value2,
                Headers = GetHeaders()
            };
        }

        public Response Post(Request data)
        {
            return new Response
            {
                Value1 = data.Value1,
                Value2 = "POST JSON: " + data.Value2,
                Headers = GetHeaders()
            };
        }

        static string[] GetHeaders()
        {
            return WebOperationContext.Current == null
                ? null
                : WebOperationContext.Current.IncomingRequest.Headers.AllKeys.Select(x => x + ": " + WebOperationContext.Current.IncomingRequest.Headers[x]).ToArray();
        }
    }
}