using System;
using System.Net;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.MSTest
{
    [TestClass]
    public class HttpClientExceptionTests
    {
        [TestMethod]
        public void WhenHttpClientExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionOfTypeWebException()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.NotFound,
                    StatusDescriptionGet = () => "Entity not found.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "Server", "Test" } }
                };

                var innerException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);
                
                var targetException = new HttpClientException("Request Failed", innerException);

                Assert.AreSame(innerException, targetException.InnerException);
                Assert.AreEqual("Request Failed", targetException.Message);

                Assert.IsNotNull(targetException.Response);

                Assert.AreEqual(404, targetException.Response.StatusCode);
                Assert.AreEqual("Entity not found.", targetException.Response.StatusDescription);
                Assert.IsNull(targetException.Response.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), targetException.Response.LastModified);

                Assert.AreEqual(1, targetException.Response.Headers.AllKeys.Length);
                Assert.AreEqual("Server", targetException.Response.Headers.AllKeys[0]);
                Assert.AreEqual("Test", targetException.Response.Headers["Server"]);
            }
        }
    }
}
