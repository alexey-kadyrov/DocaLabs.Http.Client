using System.Runtime.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    [DataContract(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
    public class DataResponse
    {
        [DataMember]
        public int Value1 { get; set; }
        [DataMember]
        public string Value2 { get; set; }
        [DataMember]
        public string[] Headers { get; set; }
    }
}