using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Test.Services._WcfServices
{
    [ServiceContract]
    public interface ITestService2
    {
        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get?value1={value1}&value2={value2}")]
        Response Get(int value1, string value2);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Post")]
        Response Post(Request data);
    }
}
