﻿using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using Moq;

namespace DocaLabs.Http.Client.Tests._Utils
{
    class response_deserialization_test_context
    {
        protected static Mock<WebRequest> mock_request;
        protected static Mock<WebResponse> mock_response;
        protected static HttpResponseStream http_response_stream;

        protected static void Setup(string contentType, Stream stream)
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(stream);
            mock_response.Object.ContentType = contentType;
            mock_response.Object.ContentLength = stream.Length;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);

            http_response_stream = HttpResponseStream.CreateResponseStream(mock_request.Object);
        }

        protected static void SetupAsync(string contentType, Stream stream)
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(stream);
            mock_response.Object.ContentType = contentType;
            mock_response.Object.ContentLength = stream.Length;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponseAsync()).Returns(Task.FromResult(mock_response.Object));

            http_response_stream = HttpResponseStream.CreateAsyncResponseStream(mock_request.Object, CancellationToken.None).Result;
        }
    }
}
