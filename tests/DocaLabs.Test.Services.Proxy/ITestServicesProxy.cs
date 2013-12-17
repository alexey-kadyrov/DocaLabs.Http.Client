using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Test.Services.Proxy
{
    [ServiceContract]
    public interface ITestServicesProxy
    {
        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "Start")]
        void Start();

        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "Stop")]
        void Stop();
    }
}
