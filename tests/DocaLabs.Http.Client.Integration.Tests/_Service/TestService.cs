using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Http.Client.Integration.Tests._Service
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TestService : ITestService
    {
        public DataResponse GetAsJson(int value1, string value2)
        {
            return new DataResponse
            {
                Value1 = value1,
                Value2 = "GET JSON: " + value2,
                Headers = GetHeaders()
            };
        }

        public DataResponse PostAsJson(DataRequest data)
        {
            return new DataResponse
            {
                Value1 = data.Value1,
                Value2 = "POST JSON: " + data.Value2,
                Headers = GetHeaders()
            };
        }

        public DataResponse GetAsXml(int value1, string value2)
        {
            return new DataResponse
            {
                Value1 = value1,
                Value2 = "GET XML: " + value2,
                Headers = GetHeaders()
            };
        }

        public DataResponse PostAsXml(DataRequest data)
        {
            return new DataResponse
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