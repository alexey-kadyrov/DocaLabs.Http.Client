using System.Xml.Serialization;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Test.Services._ServiceStackServices")]
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
