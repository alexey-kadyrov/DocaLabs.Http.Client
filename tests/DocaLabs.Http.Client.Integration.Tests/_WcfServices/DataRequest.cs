﻿using System.Runtime.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._WcfServices
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._Service")]
    public class DataRequest
    {
        [DataMember]
        public int Value1 { get; set; }

        [DataMember]
        public string Value2 { get; set; }
    }
}