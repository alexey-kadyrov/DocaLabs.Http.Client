using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    [XmlRoot(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
    public class DataRequest
    {
        public int Value1 { get; set; }
        public string Value2 { get; set; }
    }
}