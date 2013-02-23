using DocaLabs.Http.Client.Integration.Tests._Service;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    public interface ITestGetService1
    {
        OutData GetData(InData query);
    }
}