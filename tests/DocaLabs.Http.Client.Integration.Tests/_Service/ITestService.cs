﻿using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Http.Client.Integration.Tests._Service
{
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
