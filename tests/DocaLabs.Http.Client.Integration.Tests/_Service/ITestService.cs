using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._Service
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    public class InData
    {
        [DataMember]
        public int Value1 { get; set; }

        [DataMember]
        public string Value2 { get; set; }
    }

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    public class OutData
    {
        [DataMember]
        public int Value1 { get; set; }

        [DataMember]
        public string Value2 { get; set; }

        [DataMember]
        public string[] Headers { get; set; }
    }

    [ServiceContract]
    public interface ITestService
    {
        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetAsJson?value1={value1}&value2={value2}")]
        OutData GetAsJson(int value1, string value2);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml, UriTemplate = "PostAsJson")]
        OutData PostAsJson(InData data);

        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "GetAsXml?value1={value1}&value2={value2}")]
        OutData GetAsXml(int value1, string value2);

        [OperationContract, WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Json, UriTemplate = "PostAsXml")]
        OutData PostAsXml(InData data);
    }
}
