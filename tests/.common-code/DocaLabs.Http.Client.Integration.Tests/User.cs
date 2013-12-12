using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/DocaLabs.Test.Services._ServiceStackServices")]
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static User GetExpected(long id)
        {
            return new HttpClient<Request, User>(null, "getUserV2").Execute(new Request {Id = id});
        }

        class Request
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable UnusedMember.Local
            public long Id { get; set; }
            public string Format { get { return "json"; } }
            // ReSharper restore UnusedMember.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}