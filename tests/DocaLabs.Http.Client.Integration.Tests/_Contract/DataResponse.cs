using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    public class DataResponse
    {
        public int Value1 { get; set; }
        public string Value2 { get; set; }
        public string[] Headers { get; set; }
    }
}