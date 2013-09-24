﻿using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocaLabs.Http.Client.Integration.Tests._WcfServices
{
    [ServiceContract]
    public interface ITestService2
    {
        [OperationContract, WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Get?value1={value1}&value2={value2}")]
        DataResponse Get(int value1, string value2);
    }
}
