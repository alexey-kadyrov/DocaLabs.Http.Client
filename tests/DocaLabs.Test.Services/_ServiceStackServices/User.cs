using System;
using System.Xml.Serialization;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    [XmlRoot(Namespace = "http://docalabshttpclient.codeplex.com/test/data")]
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
