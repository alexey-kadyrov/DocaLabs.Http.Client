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
            return HttpClientFactory.CreateInstance<IGetUserService>("getUserV2").Get(id);
        }

        public interface IGetUserService
        {
            User Get(long id, string format = "json");
        }
    }
}