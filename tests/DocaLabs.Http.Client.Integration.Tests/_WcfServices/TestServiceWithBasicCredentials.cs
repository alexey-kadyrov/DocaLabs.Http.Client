using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Http.Client.Integration.Tests._WcfServices
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TestServiceWithBasicCredentials : ITestService2
    {
        public DataResponse Get(int value1, string value2)
        {
            return new DataResponse
            {
                Value1 = value1,
                Value2 = "GET JSON: " + value2,
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