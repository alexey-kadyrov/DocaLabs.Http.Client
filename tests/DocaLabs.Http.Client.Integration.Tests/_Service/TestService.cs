using System.ServiceModel;

namespace DocaLabs.Http.Client.Integration.Tests._Service
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class TestService : ITestService
    {
        public OutData GetAsJson(int value1, string value2)
        {
            //OperationContext.Current.IncomingMessageHeaders.Count
            return new OutData
            {
                Value1 = value1,
                Value2 = "GET JSON: " + value2
            };
        }

        public OutData PostAsJson(InData data)
        {
            return new OutData
            {
                Value1 = data.Value1,
                Value2 = "POST JSON: " + data.Value2
            };
        }

        public OutData GetAsXml(int value1, string value2)
        {
            return new OutData
            {
                Value1 = value1,
                Value2 = "GET XML: " + value2
            };
        }

        public OutData PostAsXml(InData data)
        {
            return new OutData
            {
                Value1 = data.Value1,
                Value2 = "POST XML: " + data.Value2
            };
        }
    }
}