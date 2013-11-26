using System.Runtime.Serialization;

namespace DocaLabs.Test.Services._WcfServices
{
    [DataContract(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
    public class Request
    {
        [DataMember]
        public int Value1 { get; set; }

        [DataMember]
        public string Value2 { get; set; }
    }
}