using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Test.Services._WcfServices
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TestService : ITestService
    {
        public Response GetAsJson(int value1, string value2)
        {
            return new Response
            {
                Value1 = value1,
                Value2 = "GET JSON: " + value2,
                Headers = GetHeaders()
            };
        }

        public Response PostAsJson(Request data)
        {
            return new Response
            {
                Value1 = data.Value1,
                Value2 = "POST JSON: " + data.Value2,
                Headers = GetHeaders()
            };
        }

        public Response EmptyPost(int value1, string value2)
        {
            return new Response
            {
                Value1 = value1,
                Value2 = "POST EMPTY: " + value2,
                Headers = GetHeaders()
            };
        }

        public Response GetAsXml(int value1, string value2)
        {
            return new Response
            {
                Value1 = value1,
                Value2 = "GET XML: " + value2,
                Headers = GetHeaders()
            };
        }

        public Response PostAsXml(Request data)
        {
            return new Response
            {
                Value1 = data.Value1,
                Value2 = "POST XML: " + data.Value2,
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