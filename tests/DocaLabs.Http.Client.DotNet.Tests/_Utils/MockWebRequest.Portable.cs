using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class MockWebRequest : WebRequest
    {
        readonly MockWebResponse _expectedResponse;

        public override int Timeout { get; set; }

        public MockWebRequest(MockWebResponse expectedResponse)
        {
            _expectedResponse = expectedResponse;
        }

        public override WebResponse GetResponse()
        {
            return _expectedResponse;
        }

        public override Task<WebResponse> GetResponseAsync()
        {
            return Task.FromResult((WebResponse)_expectedResponse);
        }
    }
}
