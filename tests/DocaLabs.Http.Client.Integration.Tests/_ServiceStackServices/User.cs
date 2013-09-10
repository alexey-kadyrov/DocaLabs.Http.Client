using System;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Http.Client.Integration.Tests._ServiceStackServices")]
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
