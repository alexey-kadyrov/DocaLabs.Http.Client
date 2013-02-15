using System.IO;
using System.Net;
using Moq;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class response_deserialization_test_context
    {
        protected static Mock<WebRequest> mock_request;
        protected static Mock<WebResponse> mock_response;
        protected static HttpResponse http_response;

        protected static void Setup(string contentType, Stream stream)
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(stream);
            mock_response.Object.ContentType = contentType;
            mock_response.Object.ContentLength = stream.Length;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);

            http_response = new HttpResponse(mock_request.Object);
        }
    }
}
