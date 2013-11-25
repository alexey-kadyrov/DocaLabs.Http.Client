using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Test.Services._WcfServices
{
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetAsJson?value1={value1}&value2={value2}")]
        DataResponse GetAsJson(int value1, string value2);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "PostAsJson")]
        DataResponse PostAsJson(DataRequest data);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "EmptyPost?value1={value1}&value2={value2}")]
        DataResponse EmptyPost(int value1, string value2);

        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "GetAsXml?value1={value1}&value2={value2}")]
        DataResponse GetAsXml(int value1, string value2);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "PostAsXml")]
        DataResponse PostAsXml(DataRequest data);
    }
}
