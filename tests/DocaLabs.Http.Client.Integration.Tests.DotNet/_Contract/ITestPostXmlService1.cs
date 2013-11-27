using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet._Contract
{
    [SerializeAsXml]
    public interface ITestPostXmlService1
    {
        DataResponse PostData(DataRequest query);
    }
}