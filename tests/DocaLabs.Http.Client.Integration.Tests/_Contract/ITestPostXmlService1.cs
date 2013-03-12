using DocaLabs.Http.Client.Binding.RequestSerialization;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    [SerializeAsXml]
    public interface ITestPostXmlService1
    {
        DataResponse PostData(DataRequest query);
    }
}